using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utils
{
    class Util
    {        
        [DllImport("kernel32.dll")]
        public static extern int CreateWaitableTimer(int lpTimerAttributes, bool bManualReset, int lpTimerName);

        [DllImport("kernel32.dll")]
        public static extern bool SetWaitableTimer(int hTimer, ref long pDueTime,
            int lPeriod, int pfnCompletionRoutine, // TimerCompleteDelegate
            int lpArgToCompletionRoutine, bool fResume);

        [DllImport("user32.dll")]
        public static extern bool MsgWaitForMultipleObjects(uint nCount, ref int pHandles,
            bool bWaitAll, int dwMilliseconds, uint dwWakeMask);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(int hObject);

        public const int NULL = 0;
        public const int QS_TIMER = 0x10;

        [DllImport("kernel32.dll")]
        private static extern int LoadLibrary(string lpAppName);
        [DllImport("kernel32.dll", EntryPoint = "GetModuleFileNameA")]
        private static extern int GetModuleFileName(int rlib, byte[] lptName, int lenName);
        [DllImport("kernel32.dll")]
        private static extern int FreeLibrary(int libHandle);

        private static string DLL_NAME = "FactoryTools.exe";

        static Util instance = new Util();

        public static Util getInstance()
        {
            return instance;
        }

        /// <summary>
        /// 延迟us级别
        /// </summary>
        /// <param name="us"></param>
        public static void UsDelay(int us)
        {
            long duetime = -10 * us;
            int hWaitTimer = CreateWaitableTimer(NULL, true, NULL);
            SetWaitableTimer(hWaitTimer, ref duetime, 0, NULL, NULL, false);
            while (MsgWaitForMultipleObjects(1, ref hWaitTimer, false, Timeout.Infinite, QS_TIMER)) ;
            CloseHandle(hWaitTimer);
        }


        //将结构体转换成数组
        public static byte[] StructToBytes(object structObj)
        {
            //得到结构体的大小
            int size = Marshal.SizeOf(structObj);
            //创建byte数组
            byte[] bytes = new byte[size];
            //分配结构体大小的内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            //将结构体拷到分配好的内存空间
            Marshal.StructureToPtr(structObj, structPtr, false);
            //从内存空间拷到byte数组
            Marshal.Copy(structPtr, bytes, 0, size);
            //释放内存空间
            Marshal.FreeHGlobal(structPtr);
            //返回byte数组
            return bytes;
        }


        //将数组转换成结构体
        public static object BytesToStuct(byte[] bytes, int pos,Type type)
        {
            //得到结构体的大小
            int size = Marshal.SizeOf(type);
            //byte数组长度小于结构体的大小
            if (size > bytes.Length+pos)
            {
                //返回空
                return null;
            }
            //分配结构体大小的内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            //将byte数组拷到分配好的内存空间
            Marshal.Copy(bytes, pos, structPtr, size);
            //将内存空间转换为目标结构体
            object obj = Marshal.PtrToStructure(structPtr, type);
            //释放内存空间
            Marshal.FreeHGlobal(structPtr);
            //返回结构体
            return obj;
        }

        //bytes数组转换成IntPtr指针
        public static IntPtr BytesToIntptr(byte[] bytes)
        {
            int size = bytes.Length;
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, buffer, size);
                return buffer;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }


        //IntPtr转换为数组
        public static T[] IntPtrToStructArray<T>(IntPtr ptr, int resultLen)
        {
            T[] result = new T[resultLen];
            int objSize = Marshal.SizeOf<T>();
            for (int i = 0; i < resultLen; i++)
            {
                if (IntPtr.Size == sizeof(UInt32))
                    result[i] = Marshal.PtrToStructure<T>(new IntPtr(ptr.ToInt32() + i * objSize));
                else
                    result[i] = Marshal.PtrToStructure<T>(new IntPtr(ptr.ToInt64() + i * objSize));
            }
            return result;
        }


        /// <summary>
        /// 计算4bytes校验和
        /// </summary>
        /// <param name="value"></param>
        /// <param name="preCheckSum"></param>
        /// <returns></returns>
        public static UInt32 getCheckSum(byte[] value,int len,UInt32 preCheckSum)
        {
            UInt32 newCheckSum = preCheckSum;
            for (int i=0;i< len; i++)
            {
                newCheckSum += value[i];
            }

            return newCheckSum;
        }

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <returns></returns>
        public string GetCurDir()
        {
            int num = LoadLibrary(DLL_NAME);
            byte[] array = new byte[1024];
            int moduleFileName = GetModuleFileName(num, array, 1024);
            string text = Encoding.Default.GetString(array, 0, moduleFileName);
            int num2 = text.LastIndexOf('\\');
            if (num2 > 0)
            {
                text = text.Substring(0, num2 + 1);
            }
            FreeLibrary(num);
            return text;
        }
    }
}
