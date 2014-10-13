CREATE DATABASE  IF NOT EXISTS `grit` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `grit`;
-- MySQL dump 10.13  Distrib 5.6.13, for Win32 (x86)
--
-- Host: 127.0.0.1    Database: grit
-- ------------------------------------------------------
-- Server version	5.6.19

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `ace_demo_account`
--

DROP TABLE IF EXISTS `ace_demo_account`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_demo_account` (
  `AccountId` int(11) NOT NULL,
  `Amount` decimal(20,2) NOT NULL,
  PRIMARY KEY (`AccountId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_demo_account`
--

LOCK TABLES `ace_demo_account` WRITE;
/*!40000 ALTER TABLE `ace_demo_account` DISABLE KEYS */;
INSERT INTO `ace_demo_account` VALUES (1,201.00),(2,9799.00);
/*!40000 ALTER TABLE `ace_demo_account` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ace_demo_account_activity`
--

DROP TABLE IF EXISTS `ace_demo_account_activity`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_demo_account_activity` (
  `ActivityId` int(11) NOT NULL AUTO_INCREMENT,
  `FromAccountId` int(11) DEFAULT NULL,
  `ToAccountId` int(11) DEFAULT NULL,
  `Amount` decimal(20,2) NOT NULL,
  PRIMARY KEY (`ActivityId`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_demo_account_activity`
--

LOCK TABLES `ace_demo_account_activity` WRITE;
/*!40000 ALTER TABLE `ace_demo_account_activity` DISABLE KEYS */;
INSERT INTO `ace_demo_account_activity` VALUES (18,2,1,100.00),(19,2,1,1.00),(20,2,1,100.00);
/*!40000 ALTER TABLE `ace_demo_account_activity` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ace_demo_investment`
--

DROP TABLE IF EXISTS `ace_demo_investment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_demo_investment` (
  `InvestmentId` int(11) NOT NULL,
  `ProjectId` int(11) NOT NULL,
  `AccountId` int(11) NOT NULL,
  `Amount` decimal(20,2) NOT NULL,
  `Status` int(11) NOT NULL,
  PRIMARY KEY (`InvestmentId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_demo_investment`
--

LOCK TABLES `ace_demo_investment` WRITE;
/*!40000 ALTER TABLE `ace_demo_investment` DISABLE KEYS */;
INSERT INTO `ace_demo_investment` VALUES (128,1,2,100.00,1),(138,1,2,1.00,1),(141,1,2,100.00,0),(142,1,2,100.00,0),(143,1,2,100.00,1);
/*!40000 ALTER TABLE `ace_demo_investment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ace_demo_message`
--

DROP TABLE IF EXISTS `ace_demo_message`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_demo_message` (
  `MessageId` int(11) NOT NULL AUTO_INCREMENT,
  `AccountId` int(11) NOT NULL,
  `Content` varchar(1024) NOT NULL,
  PRIMARY KEY (`MessageId`)
) ENGINE=InnoDB AUTO_INCREMENT=164 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_demo_message`
--

LOCK TABLES `ace_demo_message` WRITE;
/*!40000 ALTER TABLE `ace_demo_message` DISABLE KEYS */;
INSERT INTO `ace_demo_message` VALUES (145,2,'投资成功，投资金额100.00元。'),(146,2,'投资成功，投资金额100.00元。'),(147,2,'账户变动，变动金额-100.00元。'),(148,1,'账户变动，变动金额100.00元。'),(149,2,'账户变动，变动金额-100.00元。'),(150,1,'账户变动，变动金额100.00元。'),(151,2,'投资成功，投资金额1.00元。'),(152,2,'投资成功，投资金额1.00元。'),(153,2,'账户变动，变动金额-1.00元。'),(154,1,'账户变动，变动金额1.00元。'),(155,2,'账户变动，变动金额-1.00元。'),(156,1,'账户变动，变动金额1.00元。'),(157,2,'投资成功，投资金额100.00元。'),(158,2,'投资成功，投资金额100.00元。'),(159,2,'投资成功，投资金额100.00元。'),(160,2,'投资成功，投资金额100.00元。'),(161,2,'投资成功，投资金额100.00元。'),(162,2,'账户变动，变动金额-100.00元。'),(163,1,'账户变动，变动金额100.00元。');
/*!40000 ALTER TABLE `ace_demo_message` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ace_demo_project`
--

DROP TABLE IF EXISTS `ace_demo_project`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_demo_project` (
  `ProjectId` int(11) NOT NULL,
  `Name` varchar(100) NOT NULL,
  `Amount` decimal(20,2) NOT NULL,
  `BorrowerId` int(11) NOT NULL,
  PRIMARY KEY (`ProjectId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_demo_project`
--

LOCK TABLES `ace_demo_project` WRITE;
/*!40000 ALTER TABLE `ace_demo_project` DISABLE KEYS */;
INSERT INTO `ace_demo_project` VALUES (1,'Test Project',9900.00,1);
/*!40000 ALTER TABLE `ace_demo_project` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sequence`
--

DROP TABLE IF EXISTS `sequence`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sequence` (
  `Id` int(11) NOT NULL,
  `Value` int(11) NOT NULL DEFAULT '0',
  `Comment` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sequence`
--

LOCK TABLES `sequence` WRITE;
/*!40000 ALTER TABLE `sequence` DISABLE KEYS */;
INSERT INTO `sequence` VALUES (1,4155,'Test'),(100,3,'ace_Account'),(101,2,'ace_Project'),(102,144,'ace_Investment');
/*!40000 ALTER TABLE `sequence` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'grit'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2014-07-07  9:39:07
