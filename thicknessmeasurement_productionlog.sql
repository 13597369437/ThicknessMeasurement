-- MySQL dump 10.13  Distrib 8.0.27, for Win64 (x86_64)
--
-- Host: localhost    Database: thicknessmeasurement
-- ------------------------------------------------------
-- Server version	8.0.27

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
  `datee` datetime DEFAULT NULL,
  `MaterialNum` varchar(40) DEFAULT NULL,
  `LotNum` varchar(40) DEFAULT NULL,
  `settingThickness` float DEFAULT NULL,
  `result` varchar(20) DEFAULT NULL,
  `max` float DEFAULT NULL,
  `min` float DEFAULT NULL,
  `max-min` float DEFAULT NULL,
  `avg` float DEFAULT NULL,
  `totalcount` float DEFAULT NULL,
  `11` float DEFAULT NULL,
  `12` float DEFAULT NULL,
  `13` float DEFAULT NULL,
  `14` float DEFAULT NULL,
  `21` float DEFAULT NULL,
  `22` float DEFAULT NULL,
  `23` float DEFAULT NULL,
  `24` float DEFAULT NULL,
  `31` float DEFAULT NULL,
  `32` float DEFAULT NULL,
  `33` float DEFAULT NULL,
  `34` float DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='测厚数据记录';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `productionlog`
--

LOCK TABLES `productionlog` WRITE;
/*!40000 ALTER TABLE `productionlog` DISABLE KEYS */;
/*!40000 ALTER TABLE `productionlog` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2022-12-28  9:10:52
