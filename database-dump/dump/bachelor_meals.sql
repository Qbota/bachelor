-- MySQL dump 10.13  Distrib 8.0.15, for Win64 (x86_64)
--
-- Host: localhost    Database: bachelor
-- ------------------------------------------------------
-- Server version	8.0.15

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
 SET NAMES utf8 ;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `meals`
--

DROP TABLE IF EXISTS `meals`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `meals` (
  `meal_id` int(11) NOT NULL AUTO_INCREMENT,
  `meal_name` varchar(45) DEFAULT NULL,
  `meal_price` double DEFAULT NULL,
  `restaurant_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`meal_id`),
  KEY `FK_meals_restaurants_idx` (`restaurant_id`),
  CONSTRAINT `FK_meals_restaurants` FOREIGN KEY (`restaurant_id`) REFERENCES `restaurants` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=62 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `meals`
--

LOCK TABLES `meals` WRITE;
/*!40000 ALTER TABLE `meals` DISABLE KEYS */;
INSERT INTO `meals` VALUES (28,'Pizza Margherita',25,14),(29,'Pizza Pepperoni',25,14),(30,'Pizza Napoli',25,14),(31,'Pizza Americana',30,14),(32,'Pizza Capriciosa',28,14),(33,'Rosół',10,15),(34,'Gulasz',20,15),(35,'Schabowy',20,15),(36,'Bigos',18,15),(37,'Golonka',25,15),(38,'Kebab Baranina',12,16),(39,'Kebab Kurczak',12,16),(40,'Kebab Wege',10,16),(41,'Kebab i Frytki',15,16),(42,'Kebab XXL',15,16),(43,'Chaczapuri Mchliani',15,17),(44,'Chaczapuri Guruli',12,17),(45,'Chaczapuri Megrelskie',15,17),(46,'Chaczapuri fenowani',16,17),(47,'Lobiani',10,17),(48,'Burger Wołowy',15,18),(49,'Burger z Kurczakiem',15,18),(50,'Burger Wege',13,18),(51,'Burger i Frytki',20,18),(52,'Burger XXL',20,18),(53,'Gulasz',13,19),(54,'Schabowy',15,19),(55,'Pomidorowa',8,19),(56,'Kluski leniwe',10,19),(57,'Pierogi Ruskie',15,19),(59,'Pizza Capriciosa',21,15),(60,'Pizza Capriciosa',21,16),(61,'Pizza Capriciosa',21,17);
/*!40000 ALTER TABLE `meals` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2020-01-16 19:43:20
