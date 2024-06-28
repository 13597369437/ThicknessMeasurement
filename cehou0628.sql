CREATE DATABASE  IF NOT EXISTS `thicknessmeasurement` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `thicknessmeasurement`;
-- MySQL dump 10.13  Distrib 8.0.36, for Win64 (x86_64)
--
-- Host: localhost    Database: thicknessmeasurement
-- ------------------------------------------------------
-- Server version	8.0.35

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `productionlog`
--

DROP TABLE IF EXISTS `productionlog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `productionlog` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `datee` varchar(255) DEFAULT NULL,
  `MaterialNum` varchar(255) DEFAULT NULL,
  `LotNum` varchar(255) DEFAULT NULL,
  `钢板ID` varchar(255) DEFAULT NULL,
  `settingThickness` varchar(255) DEFAULT NULL,
  `settingMax` varchar(255) DEFAULT NULL,
  `settingMin` varchar(255) DEFAULT NULL,
  `settingMax_min` varchar(255) DEFAULT NULL,
  `result` varchar(255) DEFAULT NULL,
  `max` varchar(255) DEFAULT NULL,
  `min` varchar(255) DEFAULT NULL,
  `max_min` varchar(255) DEFAULT NULL,
  `avg` varchar(255) DEFAULT NULL,
  `totalcount` int NOT NULL,
  `d11` varchar(255) DEFAULT NULL,
  `d12` varchar(255) DEFAULT NULL,
  `d13` varchar(255) DEFAULT NULL,
  `d14` varchar(255) DEFAULT NULL,
  `d21` varchar(255) DEFAULT NULL,
  `d22` varchar(255) DEFAULT NULL,
  `d23` varchar(255) DEFAULT NULL,
  `d24` varchar(255) DEFAULT NULL,
  `d31` varchar(255) DEFAULT NULL,
  `d32` varchar(255) DEFAULT NULL,
  `d33` varchar(255) DEFAULT NULL,
  `d34` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=54 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `productionlog`
--

LOCK TABLES `productionlog` WRITE;
/*!40000 ALTER TABLE `productionlog` DISABLE KEYS */;
INSERT INTO `productionlog` VALUES (1,'2024/6/26 14:48:24','','','','0.54','0.594','0.486','0.3','OK','0.6118','0.5062','0.1056','0.5496',0,'0.5288','0.5594','0.5366','0.5519','0.5559','0.6118','0.5441','0.5452','0.5177','0.5136','0.5062','0.5050'),(2,'2024/6/26 16:10:18','','','','0.54','0.567','0.513','0.3','NG','0.6554','0.5090','0.1464','0.5533',0,'0.5452','0.5513','0.5527','0.5617','0.5743','0.5526','0.6554','0.6427','0.5138','0.5134','0.5090','0.5092'),(3,'2024/6/26 16:22:57','','','','0.54','0.567','0.513','0.3','NG','0.5806','0.4976','0.0830','0.5332',0,'0.5440','0.5527','0.5530','0.5627','0.5806','0.5580','0.5485','0.5566','0.5142','0.5174','0.4976','0.5133'),(4,'2024/6/26 16:23:09','','','','0.54','0.567','0.513','0.3','NG','0.5635','0.5031','0.0604','0.5405',0,'0.5500','0.5447','0.5540','0.5655','0.5572','0.5602','0.5635','0.5703','0.5031','0.5116','0.5137','0.4607'),(5,'2024/6/26 16:23:31','','','','0.54','0.567','0.513','0.3','NG','0.5662','0.2653','0.3009','0.5252',0,'0.5479','0.5523','0.5617','0.5477','0.5634','0.5662','0.5645','0.5500','0.5155','0.2653','0.4548','0.5160'),(6,'2024/6/26 16:23:41','','','','0.54','0.567','0.513','0.3','NG','0.5720','0.4886','0.0834','0.5358',0,'0.5529','0.5577','0.5606','0.5628','0.5720','0.5611','0.5678','0.5669','0.5175','0.5120','0.4886','0.5216'),(7,'2024/6/26 16:24:08','','','','0.54','0.567','0.513','0.3','NG','0.5805','0.5058','0.0747','0.5435',0,'0.5535','0.5535','0.5669','0.5524','0.5805','0.5683','0.5660','0.5476','0.5058','0.5107','0.5112','0.5167'),(8,'2024/6/26 16:24:31','','','','0.54','0.567','0.513','0.3','NG','0.5651','0.4981','0.0670','0.5429',0,'0.5651','0.5434','0.5462','0.5480','0.5595','0.5541','0.5649','0.5630','0.5205','0.4981','0.5091','0.5050'),(9,'2024/6/26 16:24:38','','','','0.54','0.567','0.513','0.3','OK','0.5664','0.5052','0.0612','0.5429',0,'0.5517','0.5494','0.5553','0.5620','0.5610','0.5664','0.5617','0.5634','0.5167','0.5153','0.5052','0.5143'),(10,'2024/6/26 16:24:43','','','','0.54','0.567','0.513','0.3','OK','0.5718','0.5159','0.0559','0.5419',0,'0.5386','0.5536','0.5574','0.5541','0.5718','0.5646','0.5541','0.5646','0.5159','0.5170','0.5193','0.5154'),(11,'2024/6/26 16:24:48','','','','0.54','0.567','0.513','0.3','NG','0.5688','0.5091','0.0597','0.5436',0,'0.5549','0.5589','0.5440','0.5654','0.5623','0.5628','0.5688','0.5618','0.5091','0.5124','0.5145','0.5195'),(12,'2024/6/26 16:24:51','','','','0.54','0.567','0.513','0.3','NG','0.5658','0.5024','0.0634','0.5428',0,'0.5577','0.5494','0.5499','0.5272','0.5658','0.5614','0.5592','0.5577','0.5024','0.5120','0.5191','0.5176'),(13,'2024/6/26 16:24:55','','','','0.54','0.567','0.513','0.3','OK','0.5681','0.5135','0.0546','0.5428',0,'0.5555','0.5540','0.5585','0.5526','0.5662','0.5655','0.5681','0.5636','0.5246','0.5151','0.5135','0.5048'),(14,'2024/6/26 16:24:59','','','','0.54','0.567','0.513','0.3','NG','0.5748','0.5072','0.0676','0.5427',0,'0.5556','0.5430','0.5506','0.5569','0.5642','0.5748','0.5686','0.5560','0.5142','0.5072','0.5200','0.5142'),(15,'2024/6/26 16:25:03','','','','0.54','0.567','0.513','0.3','NG','0.5737','0.5038','0.0699','0.5420',0,'0.5350','0.5549','0.5481','0.5515','0.5620','0.5737','0.5614','0.5633','0.5038','0.5091','0.5168','0.5105'),(16,'2024/6/26 16:25:07','','','','0.54','0.567','0.513','0.3','OK','0.5665','0.5134','0.0531','0.5425',0,'0.5516','0.5527','0.5410','0.5422','0.5576','0.5625','0.5665','0.5614','0.5134','0.5139','0.5151','0.5114'),(17,'2024/6/26 16:25:11','','','','0.54','0.567','0.513','0.3','OK','0.5664','0.5207','0.0457','0.5418',0,'0.5579','0.5478','0.5468','0.5444','0.5484','0.5562','0.5664','0.5629','0.5243','0.5247','0.5207','0.5155'),(18,'2024/6/26 16:25:15','','','','0.54','0.567','0.513','0.3','NG','0.5659','0.5106','0.0553','0.5432',0,'0.5459','0.5580','0.5591','0.5454','0.5626','0.5659','0.5654','0.5632','0.5107','0.5131','0.5106','0.5143'),(19,'2024/6/26 16:25:20','','','','0.54','0.567','0.513','0.3','OK','0.5638','0.5147','0.0491','0.5433',0,'0.5589','0.5638','0.5390','0.5412','0.5615','0.5507','0.5588','0.5603','0.5147','0.5215','0.5167','0.5098'),(20,'2024/6/26 16:25:26','','','','0.54','0.567','0.513','0.3','NG','0.5699','0.5102','0.0597','0.5436',0,'0.5588','0.5532','0.5573','0.5562','0.5699','0.5611','0.5594','0.5706','0.5111','0.5102','0.5123','0.5098'),(21,'2024/6/26 16:37:14','','','','1.64','1.804','1.476','0.3','OK','1.6719','1.6253','0.0466','1.6479',0,'1.6267','1.6253','1.6284','1.6250','1.6527','1.6427','1.6528','1.6464','1.6719','1.6643','1.6692','1.6722'),(22,'2024/6/26 16:37:26','','','','1.64','1.804','1.476','0.3','OK','1.6865','1.6010','0.0855','1.6481',0,'1.6255','1.6260','1.6010','1.6298','1.6455','1.6535','1.6548','1.6470','1.6691','1.6664','1.6865','1.6650'),(23,'2024/6/26 16:37:31','','','','1.64','1.804','1.476','0.3','OK','1.6751','1.6208','0.0543','1.6479',0,'1.6208','1.6267','1.6309','1.6259','1.6510','1.6404','1.6459','1.6439','1.6650','1.6687','1.6751','1.6672'),(24,'2024/6/26 16:37:35','','','','1.64','1.804','1.476','0.3','OK','1.6702','1.6232','0.0470','1.6479',0,'1.6283','1.6257','1.6232','1.6249','1.6545','1.6520','1.6487','1.6508','1.6702','1.6693','1.6652','1.6742'),(25,'2024/6/26 16:37:42','','','','1.64','1.804','1.476','0.3','OK','1.6702','1.6244','0.0458','1.6468',0,'1.6271','1.6244','1.6268','1.6264','1.6510','1.6450','1.6457','1.6447','1.6702','1.6702','1.6660','1.6674'),(26,'2024/6/26 16:37:46','','','','1.64','1.804','1.476','0.3','OK','1.6671','1.6260','0.0411','1.6462',0,'1.6260','1.6261','1.6264','1.6254','1.6457','1.6447','1.6600','1.6495','1.6671','1.6660','1.6647','1.6668'),(27,'2024/6/26 16:37:52','','','','1.64','1.804','1.476','0.3','OK','1.6693','1.6184','0.0509','1.6464',0,'1.6286','1.6184','1.6228','1.6274','1.6464','1.6486','1.6443','1.6446','1.6680','1.6693','1.6653','1.6738'),(28,'2024/6/26 16:37:58','','','','1.64','1.804','1.476','0.3','OK','1.6678','1.6266','0.0412','1.6467',0,'1.6283','1.6266','1.6282','1.6274','1.6498','1.6507','1.6473','1.6498','1.6606','1.6668','1.6678','1.6702'),(29,'2024/6/26 16:38:03','','','','1.64','1.804','1.476','0.3','OK','1.6707','1.6237','0.0470','1.6473',0,'1.6237','1.6273','1.6273','1.6265','1.6454','1.6503','1.6476','1.6436','1.6668','1.6707','1.6698','1.6667'),(30,'2024/6/26 16:38:07','','','','1.64','1.804','1.476','0.3','OK','1.6719','1.6222','0.0497','1.6485',0,'1.6286','1.6308','1.6222','1.6428','1.6456','1.6464','1.6524','1.6440','1.6676','1.6713','1.6719','1.6815'),(31,'2024/6/26 16:38:11','','','','1.64','1.804','1.476','0.3','OK','1.6726','1.6216','0.0510','1.6479',0,'1.6283','1.6252','1.6216','1.6318','1.6510','1.6442','1.6400','1.6495','1.6692','1.6726','1.6658','1.6716'),(32,'2024/6/26 16:38:14','','','','1.64','1.804','1.476','0.3','OK','1.6713','1.6262','0.0451','1.6473',0,'1.6317','1.6262','1.6312','1.6242','1.6429','1.6432','1.6468','1.6454','1.6680','1.6667','1.6713','1.6687'),(33,'2024/6/26 16:38:17','','','','1.64','1.804','1.476','0.3','OK','1.6703','1.6265','0.0438','1.6471',0,'1.6265','1.6334','1.6274','1.6289','1.6422','1.6529','1.6398','1.6475','1.6672','1.6662','1.6703','1.6725'),(34,'2024/6/26 16:38:21','','','','1.64','1.804','1.476','0.3','OK','1.6724','1.6235','0.0489','1.6471',0,'1.6235','1.6274','1.6258','1.6238','1.6382','1.6474','1.6503','1.6445','1.6662','1.6724','1.6708','1.6685'),(35,'2024/6/26 16:38:25','','','','1.64','1.804','1.476','0.3','OK','1.6745','1.6244','0.0501','1.6478',0,'1.6293','1.6244','1.6281','1.6268','1.6458','1.6504','1.6456','1.6441','1.6720','1.6745','1.6677','1.6661'),(36,'2024/6/26 16:38:29','','','','1.64','1.804','1.476','0.3','OK','1.6722','1.6283','0.0439','1.6468',0,'1.6286','1.6283','1.6290','1.6281','1.6443','1.6409','1.6413','1.6458','1.6685','1.6679','1.6722','1.6692'),(37,'2024/6/26 16:38:33','','','','1.64','1.804','1.476','0.3','OK','1.6736','1.6251','0.0485','1.6469',0,'1.6268','1.6251','1.6268','1.6259','1.6460','1.6430','1.6437','1.6448','1.6733','1.6736','1.6687','1.6685'),(38,'2024/6/26 16:38:38','','','','1.64','1.804','1.476','0.3','OK','1.6720','1.6260','0.0460','1.6471',0,'1.6271','1.6260','1.6330','1.6296','1.6435','1.6442','1.6461','1.6459','1.6720','1.6672','1.6695','1.6668'),(39,'2024/6/26 16:40:34','','','','1.64','1.804','1.476','0.3','OK','1.6683','1.6268','0.0415','1.6470',0,'1.6268','1.6305','1.6346','1.6297','1.6484','1.6438','1.6445','1.6416','1.6577','1.6668','1.6683','1.6629'),(40,'2024/6/26 16:40:49','','','','1.64','1.804','1.476','0.3','OK','1.6681','1.6307','0.0374','1.6467',0,'1.6343','1.6347','1.6307','1.6411','1.6478','1.6411','1.6420','1.6486','1.6637','1.6626','1.6681','1.6676'),(41,'2024/6/26 16:41:04','','','','1.64','1.804','1.476','0.3','OK','1.6683','1.6332','0.0351','1.6509',0,'1.6336','1.6341','1.6332','1.6327','1.6500','1.6521','1.6513','1.6514','1.6683','1.6677','1.6682','1.6683'),(42,'2024/6/26 16:43:32','','','','1.64','1.804','1.476','0.3','OK','1.6644','1.6309','0.0335','1.6465',0,'1.6332','1.6309','1.6320','1.6273','1.6416','1.6455','1.6428','1.6434','1.6644','1.6630','1.6609','1.6649'),(43,'2024/6/26 16:43:42','','','','1.64','1.804','1.476','0.3','OK','1.6678','1.6328','0.0350','1.6480',0,'1.6328','1.6334','1.6360','1.6298','1.6466','1.6463','1.6497','1.6399','1.6636','1.6633','1.6678','1.6625'),(44,'2024/6/26 16:43:55','','','','1.64','1.804','1.476','0.3','OK','1.6782','1.6154','0.0628','1.6465',0,'1.6154','1.6335','1.6325','1.6366','1.6419','1.6453','1.6420','1.6479','1.6782','1.6642','1.6639','1.6665'),(45,'2024/6/26 16:44:00','','','','1.64','1.804','1.476','0.3','OK','1.6650','1.6282','0.0368','1.6462',0,'1.6352','1.6282','1.6326','1.6352','1.6465','1.6405','1.6459','1.6482','1.6650','1.6599','1.6629','1.6658'),(46,'2024/6/26 16:44:11','','','','1.64','1.804','1.476','0.3','OK','1.6642','1.6298','0.0344','1.6473',0,'1.6321','1.6355','1.6298','1.6229','1.6448','1.6482','1.6463','1.6442','1.6635','1.6642','1.6640','1.6594'),(47,'2024/6/26 16:44:15','','','','1.64','1.804','1.476','0.3','OK','1.6634','1.6275','0.0359','1.6462',0,'1.6300','1.6275','1.6342','1.6349','1.6434','1.6463','1.6433','1.6498','1.6602','1.6634','1.6614','1.6636'),(48,'2024/6/26 16:44:19','','','','1.64','1.804','1.476','0.3','OK','1.6650','1.6326','0.0324','1.6479',0,'1.6334','1.6326','1.6326','1.6338','1.6440','1.6472','1.6515','1.6411','1.6650','1.6628','1.6631','1.6655'),(49,'2024/6/26 16:49:16','','','','1.64','1.804','1.476','0.3','OK','1.6600','1.6198','0.0402','1.6432',0,'1.6246','1.6245','1.6198','1.6247','1.6487','1.6491','1.6441','1.6518','1.6581','1.6600','1.6562','1.6594'),(50,'2024/6/26 16:49:21','','','','1.64','1.804','1.476','0.3','OK','1.6592','1.6198','0.0394','1.6419',0,'1.6198','1.6209','1.6266','1.6229','1.6434','1.6439','1.6473','1.6502','1.6580','1.6586','1.6592','1.6584'),(51,'2024/6/26 16:49:33','','','','1.64','1.804','1.476','0.3','OK','1.6620','1.6249','0.0371','1.6443',0,'1.6249','1.6250','1.6252','1.6209','1.6431','1.6490','1.6503','1.6446','1.6613','1.6603','1.6620','1.6623'),(52,'2024/6/26 16:49:48','','','','1.64','1.804','1.476','0.3','OK','1.6679','1.6215','0.0464','1.6449',0,'1.6215','1.6227','1.6255','1.6219','1.6491','1.6502','1.6478','1.6523','1.6574','1.6625','1.6679','1.6572'),(53,'2024/6/26 16:50:20','','','','1.64','1.804','1.476','0.3','OK','1.6618','1.6226','0.0392','1.6433',0,'1.6236','1.6226','1.6235','1.6211','1.6437','1.6449','1.6470','1.6477','1.6598','1.6618','1.6547','1.6610');
/*!40000 ALTER TABLE `productionlog` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `测量参数`
--

DROP TABLE IF EXISTS `测量参数`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `测量参数` (
  `id` int NOT NULL,
  `项目` varchar(255) DEFAULT NULL,
  `值` varchar(255) DEFAULT NULL,
  `备注` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `测量参数`
--

LOCK TABLES `测量参数` WRITE;
/*!40000 ALTER TABLE `测量参数` DISABLE KEYS */;
INSERT INTO `测量参数` VALUES (0,'产品厚度','1.64',NULL),(1,'极差','0.3',NULL),(2,'上限','1.804',NULL),(3,'下限','1.476',NULL),(4,'上下限百分比','10',NULL),(5,'取样点1','20',NULL),(6,'取样点2','50',NULL),(7,'取样点3','80',NULL),(8,'取样点4','0',NULL),(9,'NG点数','2',NULL),(10,'折线图精度','0.4',NULL),(11,'整板取样','0',NULL),(12,'NG百分比','0.1',NULL),(13,'固定差值','0.2',NULL);
/*!40000 ALTER TABLE `测量参数` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `用户管理`
--

DROP TABLE IF EXISTS `用户管理`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `用户管理` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `用户名` varchar(255) DEFAULT NULL,
  `姓名` varchar(255) DEFAULT NULL,
  `密码` varchar(255) DEFAULT NULL,
  `权限` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `用户管理`
--

LOCK TABLES `用户管理` WRITE;
/*!40000 ALTER TABLE `用户管理` DISABLE KEYS */;
INSERT INTO `用户管理` VALUES (1,'Admin','超级管理员','123','3'),(2,'1','张三','1','1'),(3,'2','李四','2','2');
/*!40000 ALTER TABLE `用户管理` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `配置`
--

DROP TABLE IF EXISTS `配置`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `配置` (
  `id` int NOT NULL,
  `项目` varchar(255) DEFAULT NULL,
  `值` varchar(255) DEFAULT NULL,
  `备注` varchar(255) DEFAULT NULL,
  `默认值` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `配置`
--

LOCK TABLES `配置` WRITE;
/*!40000 ALTER TABLE `配置` DISABLE KEYS */;
INSERT INTO `配置` VALUES (0,'测厚仪','2','0：基恩士；1：海博森；2：深视（重启生效）','0'),(1,'识别二维码','0','0：不识别；1：识别','0'),(2,'自动识别钢板','0','0：关；1：开','0'),(3,'读码超时时间','5','单位为s','5'),(4,'读码器ip','192.168.1.1','','192.168.1.1'),(5,'读码器端口号','5000','','5000'),(6,'读码器触发频率','100','单位为ms','100'),(7,'测量超时时间','5','单位为s','5'),(8,'EAP','0','0：无EAP系统；1：有EAP系统','0'),(9,'读数精度','4','测量得到的读数显示几位小数点','4'),(10,'判定方式','1','0：极差法；1：上下限法','1'),(11,'采样点数','9','9点测量或者12点测量','9'),(12,'上下限设定','0','0：百分比法；1：固定值法','0'),(13,'整板取样','0','0：关；1：开',NULL),(14,'null',NULL,NULL,NULL),(15,'null',NULL,NULL,NULL),(16,'null',NULL,NULL,NULL),(17,'null',NULL,NULL,NULL),(18,'null',NULL,NULL,NULL),(19,'null',NULL,NULL,NULL),(20,'null',NULL,NULL,NULL),(21,'null',NULL,NULL,NULL),(22,'null',NULL,NULL,NULL),(23,'null',NULL,NULL,NULL),(24,'null',NULL,NULL,NULL),(25,'null',NULL,NULL,NULL),(26,'null',NULL,NULL,NULL),(27,'null',NULL,NULL,NULL),(28,'null',NULL,NULL,NULL),(29,'null',NULL,NULL,NULL),(30,'null',NULL,NULL,NULL),(31,'null',NULL,NULL,NULL),(32,'null',NULL,NULL,NULL),(33,'null',NULL,NULL,NULL),(34,'null',NULL,NULL,NULL),(35,'null',NULL,NULL,NULL),(36,'null',NULL,NULL,NULL),(37,'null',NULL,NULL,NULL),(38,'null',NULL,NULL,NULL),(39,'null',NULL,NULL,NULL),(40,'null',NULL,NULL,NULL),(41,'null',NULL,NULL,NULL),(42,'null',NULL,NULL,NULL),(43,'null',NULL,NULL,NULL),(44,'null',NULL,NULL,NULL),(45,'null',NULL,NULL,NULL),(46,'null',NULL,NULL,NULL),(47,'null',NULL,NULL,NULL),(48,'null',NULL,NULL,NULL),(49,'null',NULL,NULL,NULL),(50,'连接方式','1','0：以太网；1：USB','0'),(51,'IP地址','192.168.10.10','基恩士测厚仪IP地址','192.168.10.10'),(52,'中值滤波宽度','99','中值滤波的取样点数量','99'),(53,'null',NULL,NULL,NULL),(54,'null',NULL,NULL,NULL),(55,'null',NULL,NULL,NULL),(56,'null',NULL,NULL,NULL),(57,'null',NULL,NULL,NULL),(58,'null',NULL,NULL,NULL),(59,'null',NULL,NULL,NULL),(60,'null',NULL,NULL,NULL),(61,'null',NULL,NULL,NULL),(62,'null',NULL,NULL,NULL),(63,'null',NULL,NULL,NULL),(64,'null',NULL,NULL,NULL),(65,'null',NULL,NULL,NULL),(66,'null',NULL,NULL,NULL),(67,'null',NULL,NULL,NULL),(68,'null',NULL,NULL,NULL),(69,'null',NULL,NULL,NULL),(70,'null',NULL,NULL,NULL),(71,'null',NULL,NULL,NULL),(72,'null',NULL,NULL,NULL),(73,'null',NULL,NULL,NULL),(74,'null',NULL,NULL,NULL),(75,'null',NULL,NULL,NULL),(76,'null',NULL,NULL,NULL),(77,'null',NULL,NULL,NULL),(78,'null',NULL,NULL,NULL),(79,'null',NULL,NULL,NULL),(80,'null',NULL,NULL,NULL),(81,'null',NULL,NULL,NULL),(82,'null',NULL,NULL,NULL),(83,'null',NULL,NULL,NULL),(84,'null',NULL,NULL,NULL),(85,'null',NULL,NULL,NULL),(86,'null',NULL,NULL,NULL),(87,'null',NULL,NULL,NULL),(88,'null',NULL,NULL,NULL),(89,'null',NULL,NULL,NULL),(90,'null',NULL,NULL,NULL),(91,'null',NULL,NULL,NULL),(92,'null',NULL,NULL,NULL),(93,'null',NULL,NULL,NULL),(94,'null',NULL,NULL,NULL),(95,'null',NULL,NULL,NULL),(96,'null',NULL,NULL,NULL),(97,'null',NULL,NULL,NULL),(98,'null',NULL,NULL,NULL),(99,'null',NULL,NULL,NULL),(100,'null',NULL,NULL,NULL),(101,'null',NULL,NULL,NULL),(102,'null',NULL,NULL,NULL),(103,'null',NULL,NULL,NULL),(104,'null',NULL,NULL,NULL),(105,'null',NULL,NULL,NULL),(106,'null',NULL,NULL,NULL),(107,'null',NULL,NULL,NULL),(108,'null',NULL,NULL,NULL),(109,'null',NULL,NULL,NULL),(110,'null',NULL,NULL,NULL),(111,'null',NULL,NULL,NULL),(112,'null',NULL,NULL,NULL),(113,'null',NULL,NULL,NULL),(114,'null',NULL,NULL,NULL),(115,'null',NULL,NULL,NULL),(116,'null',NULL,NULL,NULL),(117,'null',NULL,NULL,NULL),(118,'null',NULL,NULL,NULL),(119,'null',NULL,NULL,NULL),(120,'null',NULL,NULL,NULL),(121,'null',NULL,NULL,NULL),(122,'null',NULL,NULL,NULL),(123,'null',NULL,NULL,NULL),(124,'null',NULL,NULL,NULL),(125,'null',NULL,NULL,NULL),(126,'null',NULL,NULL,NULL),(127,'null',NULL,NULL,NULL),(128,'null',NULL,NULL,NULL),(129,'null',NULL,NULL,NULL),(130,'null',NULL,NULL,NULL),(131,'null',NULL,NULL,NULL),(132,'null',NULL,NULL,NULL),(133,'null',NULL,NULL,NULL),(134,'null',NULL,NULL,NULL),(135,'null',NULL,NULL,NULL),(136,'null',NULL,NULL,NULL),(137,'null',NULL,NULL,NULL),(138,'null',NULL,NULL,NULL),(139,'null',NULL,NULL,NULL),(140,'null',NULL,NULL,NULL),(141,'null',NULL,NULL,NULL),(142,'null',NULL,NULL,NULL),(143,'null',NULL,NULL,NULL),(144,'null',NULL,NULL,NULL),(145,'null',NULL,NULL,NULL),(146,'null',NULL,NULL,NULL),(147,'null',NULL,NULL,NULL),(148,'null',NULL,NULL,NULL),(149,'null',NULL,NULL,NULL),(150,'PLC站号','2','MX连接PLC的站号','2'),(151,'PC心跳','W105','',''),(152,'PLC心跳','W106','',''),(153,'测量值','W108','',''),(154,'测量结果','W100','1：OK；2：NG',NULL),(155,'开始测量','W110','1：开始；2：结束',NULL),(156,'PCReady','W111','1：PCReady；2：测厚硬件连接失败',NULL),(157,'开始读码','W112','1：开始；2：读码完成；3读码超时',NULL),(158,'null',NULL,NULL,NULL),(159,'null',NULL,NULL,NULL),(160,'null',NULL,NULL,NULL),(161,'null',NULL,NULL,NULL),(162,'null',NULL,NULL,NULL),(163,'null',NULL,NULL,NULL),(164,'null',NULL,NULL,NULL),(165,'null',NULL,NULL,NULL),(166,'null',NULL,NULL,NULL),(167,'null',NULL,NULL,NULL),(168,'null',NULL,NULL,NULL),(169,'null',NULL,NULL,NULL),(170,'null',NULL,NULL,NULL),(171,'null',NULL,NULL,NULL),(172,'null',NULL,NULL,NULL),(173,'null',NULL,NULL,NULL),(174,'null',NULL,NULL,NULL),(175,'null',NULL,NULL,NULL),(176,'null',NULL,NULL,NULL),(177,'null',NULL,NULL,NULL),(178,'null',NULL,NULL,NULL),(179,'null',NULL,NULL,NULL),(180,'null',NULL,NULL,NULL),(181,'null',NULL,NULL,NULL),(182,'null',NULL,NULL,NULL),(183,'null',NULL,NULL,NULL),(184,'null',NULL,NULL,NULL),(185,'null',NULL,NULL,NULL),(186,'null',NULL,NULL,NULL),(187,'null',NULL,NULL,NULL),(188,'null',NULL,NULL,NULL),(189,'null',NULL,NULL,NULL),(190,'null',NULL,NULL,NULL),(191,'null',NULL,NULL,NULL),(192,'null',NULL,NULL,NULL),(193,'null',NULL,NULL,NULL),(194,'null',NULL,NULL,NULL),(195,'null',NULL,NULL,NULL),(196,'null',NULL,NULL,NULL),(197,'null',NULL,NULL,NULL),(198,'null',NULL,NULL,NULL),(199,'null',NULL,NULL,NULL),(200,'IP地址1','192.168.0.10','深视测厚仪IP地址1',''),(201,'IP地址2','192.168.0.11','深视测厚仪IP地址2',''),(202,'中值滤波宽度','9','中值滤波的取样点数量','9'),(203,NULL,NULL,NULL,NULL),(204,'null',NULL,NULL,NULL),(205,'null',NULL,NULL,NULL),(206,'null',NULL,NULL,NULL),(207,'null',NULL,NULL,NULL),(208,'null',NULL,NULL,NULL),(209,'null',NULL,NULL,NULL),(210,'null',NULL,NULL,NULL),(211,'null',NULL,NULL,NULL),(212,'null',NULL,NULL,NULL),(213,'null',NULL,NULL,NULL),(214,'null',NULL,NULL,NULL),(215,'null',NULL,NULL,NULL),(216,'null',NULL,NULL,NULL),(217,'null',NULL,NULL,NULL),(218,'null',NULL,NULL,NULL),(219,'null',NULL,NULL,NULL),(220,'null',NULL,NULL,NULL),(221,'null',NULL,NULL,NULL),(222,'null',NULL,NULL,NULL),(223,'null',NULL,NULL,NULL),(224,'null',NULL,NULL,NULL),(225,'null',NULL,NULL,NULL),(226,'null',NULL,NULL,NULL),(227,'null',NULL,NULL,NULL),(228,'null',NULL,NULL,NULL),(229,'null',NULL,NULL,NULL),(230,'null',NULL,NULL,NULL),(231,'null',NULL,NULL,NULL),(232,'null',NULL,NULL,NULL),(233,'null',NULL,NULL,NULL),(234,'null',NULL,NULL,NULL),(235,'null',NULL,NULL,NULL),(236,'null',NULL,NULL,NULL),(237,'null',NULL,NULL,NULL),(238,'null',NULL,NULL,NULL),(239,'null',NULL,NULL,NULL),(240,'null',NULL,NULL,NULL),(241,'null',NULL,NULL,NULL),(242,'null',NULL,NULL,NULL),(243,'null',NULL,NULL,NULL),(244,'null',NULL,NULL,NULL),(245,'null',NULL,NULL,NULL),(246,'null',NULL,NULL,NULL),(247,'null',NULL,NULL,NULL),(248,'null',NULL,NULL,NULL),(249,'null',NULL,NULL,NULL),(250,'null',NULL,NULL,NULL),(251,'null',NULL,NULL,NULL),(252,'null',NULL,NULL,NULL),(253,'null',NULL,NULL,NULL),(254,'null',NULL,NULL,NULL),(255,'null',NULL,NULL,NULL),(256,'null',NULL,NULL,NULL),(257,'null',NULL,NULL,NULL),(258,'null',NULL,NULL,NULL),(259,'null',NULL,NULL,NULL),(260,'null',NULL,NULL,NULL),(261,'null',NULL,NULL,NULL),(262,'null',NULL,NULL,NULL),(263,'null',NULL,NULL,NULL),(264,'null',NULL,NULL,NULL),(265,'null',NULL,NULL,NULL),(266,'null',NULL,NULL,NULL),(267,'null',NULL,NULL,NULL),(268,'null',NULL,NULL,NULL),(269,'null',NULL,NULL,NULL),(270,'null',NULL,NULL,NULL),(271,'null',NULL,NULL,NULL),(272,'null',NULL,NULL,NULL),(273,'null',NULL,NULL,NULL),(274,'null',NULL,NULL,NULL),(275,'null',NULL,NULL,NULL),(276,'null',NULL,NULL,NULL),(277,'null',NULL,NULL,NULL),(278,'null',NULL,NULL,NULL),(279,'null',NULL,NULL,NULL),(280,'null',NULL,NULL,NULL),(281,'null',NULL,NULL,NULL),(282,'null',NULL,NULL,NULL),(283,'null',NULL,NULL,NULL),(284,'null',NULL,NULL,NULL),(285,'null',NULL,NULL,NULL),(286,'null',NULL,NULL,NULL),(287,'null',NULL,NULL,NULL),(288,'null',NULL,NULL,NULL),(289,'null',NULL,NULL,NULL),(290,'null',NULL,NULL,NULL),(291,'null',NULL,NULL,NULL),(292,'null',NULL,NULL,NULL),(293,'null',NULL,NULL,NULL),(294,'null',NULL,NULL,NULL),(295,'null',NULL,NULL,NULL),(296,'null',NULL,NULL,NULL),(297,'null',NULL,NULL,NULL),(298,'null',NULL,NULL,NULL),(299,'null',NULL,NULL,NULL);
/*!40000 ALTER TABLE `配置` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `钢板参数`
--

DROP TABLE IF EXISTS `钢板参数`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `钢板参数` (
  `id` int NOT NULL AUTO_INCREMENT,
  `钢板ID` varchar(255) DEFAULT NULL,
  `钢板名称` varchar(255) DEFAULT NULL,
  `产品厚度` double NOT NULL,
  `自动识别范围` int NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `钢板参数`
--

LOCK TABLES `钢板参数` WRITE;
/*!40000 ALTER TABLE `钢板参数` DISABLE KEYS */;
INSERT INTO `钢板参数` VALUES (1,'1','钢板1',1,30),(2,'2','钢板2',1.5,26),(3,'3','钢板3',2,12),(4,'4','钢板4',0.5,17),(5,'5','钢板5',1.3,10);
/*!40000 ALTER TABLE `钢板参数` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `钢板记录`
--

DROP TABLE IF EXISTS `钢板记录`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `钢板记录` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `钢板ID` varchar(255) DEFAULT NULL,
  `设定次数` int NOT NULL,
  `生产次数` int NOT NULL,
  `剩余次数` int NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `钢板记录`
--

LOCK TABLES `钢板记录` WRITE;
/*!40000 ALTER TABLE `钢板记录` DISABLE KEYS */;
INSERT INTO `钢板记录` VALUES (1,'001',100,0,100),(2,'002',100,5,90),(3,'003',100,0,0),(4,'004',100,0,0),(5,'005',120,0,0),(6,'006',111,0,0);
/*!40000 ALTER TABLE `钢板记录` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-06-28 11:40:27
