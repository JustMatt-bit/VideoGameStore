-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Dec 15, 2023 at 02:25 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `videogamestore`
--

-- --------------------------------------------------------

--
-- Table structure for table `accounts`
--

CREATE TABLE `accounts` (
  `username` varchar(30) NOT NULL,
  `password` varchar(32) NOT NULL,
  `name` varchar(30) NOT NULL,
  `surname` varchar(30) NOT NULL,
  `email` varchar(100) NOT NULL,
  `phone` varchar(30) NOT NULL,
  `referal_code` varchar(30) DEFAULT NULL,
  `creation_date` datetime NOT NULL DEFAULT current_timestamp(),
  `loyalty_progress` int(11) NOT NULL DEFAULT 0,
  `fk_user_type` int(11) NOT NULL,
  `fk_loyalty_tier` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `accounts`
--

INSERT INTO `accounts` (`username`, `password`, `name`, `surname`, `email`, `phone`, `referal_code`, `creation_date`, `loyalty_progress`, `fk_user_type`, `fk_loyalty_tier`) VALUES
('dovyd', '688787d8ff144c502c7f5cffaafe2cc5', 'Dovydas', 'Katinas', 'lolpmx@gmail.com', '868564914', 'Zaibas', '2023-12-15 15:20:23', 0, 1, 1),
('JonasPonas', '2e53d715b9d776b6c45263d31ecd3d87', 'Jonas', 'Ponas', 'jonas.ponas@gmail.com', '864761351', NULL, '2023-11-03 14:34:43', 15, 1, 1);

-- --------------------------------------------------------

--
-- Table structure for table `addresses`
--

CREATE TABLE `addresses` (
  `address_id` int(11) NOT NULL,
  `city` varchar(30) NOT NULL,
  `street` varchar(30) NOT NULL,
  `building` int(11) NOT NULL,
  `postal_code` varchar(30) NOT NULL,
  `fk_account` varchar(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `addresses`
--

INSERT INTO `addresses` (`address_id`, `city`, `street`, `building`, `postal_code`, `fk_account`) VALUES
(1, 'Kaunas', 'Test 1-oji', 10, 'LT-24523', 'JonasPonas');

-- --------------------------------------------------------

--
-- Table structure for table `carts`
--

CREATE TABLE `carts` (
  `cart_id` int(11) NOT NULL,
  `price` float NOT NULL,
  `stock` int(11) NOT NULL,
  `fk_order` int(11) NOT NULL,
  `fk_product` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `carts`
--

INSERT INTO `carts` (`cart_id`, `price`, `stock`, `fk_order`, `fk_product`) VALUES
(1, 59.99, 2, 1, 1);

-- --------------------------------------------------------

--
-- Table structure for table `comments`
--

CREATE TABLE `comments` (
  `comment_id` int(11) NOT NULL,
  `date` datetime NOT NULL DEFAULT current_timestamp(),
  `text` text NOT NULL,
  `fk_account` varchar(30) DEFAULT NULL,
  `fk_feedback` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `comments`
--

INSERT INTO `comments` (`comment_id`, `date`, `text`, `fk_account`, `fk_feedback`) VALUES
(1, '2023-11-03 14:50:03', 'Tikrai tikrai', 'JonasPonas', 1);

-- --------------------------------------------------------

--
-- Table structure for table `developers`
--

CREATE TABLE `developers` (
  `developer_id` int(11) NOT NULL,
  `name` varchar(30) NOT NULL,
  `country` varchar(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `developers`
--

INSERT INTO `developers` (`developer_id`, `name`, `country`) VALUES
(1, 'CD Project RED', 'Poland');

-- --------------------------------------------------------

--
-- Table structure for table `discounts`
--

CREATE TABLE `discounts` (
  `discount_id` int(11) NOT NULL,
  `valid_from` datetime NOT NULL,
  `valid_to` datetime NOT NULL,
  `percent` int(11) NOT NULL,
  `fk_account` varchar(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `discounts`
--

INSERT INTO `discounts` (`discount_id`, `valid_from`, `valid_to`, `percent`, `fk_account`) VALUES
(1, '2023-11-03 13:43:59', '2023-11-04 14:43:59', 50, 'JonasPonas');

-- --------------------------------------------------------

--
-- Table structure for table `feedback`
--

CREATE TABLE `feedback` (
  `feedback_id` int(11) NOT NULL,
  `date` datetime NOT NULL,
  `text` text NOT NULL,
  `rating` float NOT NULL,
  `rating_count` int(11) NOT NULL,
  `flagged` tinyint(1) NOT NULL,
  `fk_account` varchar(30) DEFAULT NULL,
  `fk_product` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `feedback`
--

INSERT INTO `feedback` (`feedback_id`, `date`, `text`, `rating`, `rating_count`, `flagged`, `fk_account`, `fk_product`) VALUES
(1, '2023-11-03 13:48:38', 'Good game!', 5, 1, 0, 'JonasPonas', 1);

-- --------------------------------------------------------

--
-- Table structure for table `game_types`
--

CREATE TABLE `game_types` (
  `game_type_id` int(11) NOT NULL,
  `name` varchar(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `game_types`
--

INSERT INTO `game_types` (`game_type_id`, `name`) VALUES
(1, 'fizinis'),
(2, 'internetinis raktas');

-- --------------------------------------------------------

--
-- Table structure for table `genres`
--

CREATE TABLE `genres` (
  `genre_id` int(11) NOT NULL,
  `name` varchar(30) NOT NULL,
  `description` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `genres`
--

INSERT INTO `genres` (`genre_id`, `name`, `description`) VALUES
(1, 'First-Person Shooter', 'You shoot');

-- --------------------------------------------------------

--
-- Table structure for table `loyalty_tiers`
--

CREATE TABLE `loyalty_tiers` (
  `tier_id` int(11) NOT NULL,
  `name` varchar(30) NOT NULL,
  `points_from` int(11) NOT NULL,
  `points_to` int(11) NOT NULL,
  `description` text NOT NULL,
  `discount_coeficient` float NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `loyalty_tiers`
--

INSERT INTO `loyalty_tiers` (`tier_id`, `name`, `points_from`, `points_to`, `description`, `discount_coeficient`) VALUES
(1, 'Pirmas lygis', 0, 10, 'test test test', 1);

-- --------------------------------------------------------

--
-- Table structure for table `orders`
--

CREATE TABLE `orders` (
  `order_id` int(11) NOT NULL,
  `creation_date` datetime NOT NULL DEFAULT current_timestamp(),
  `completion_date` datetime NOT NULL,
  `price` float NOT NULL DEFAULT 0,
  `comment` text NOT NULL,
  `parcel_price` float NOT NULL DEFAULT 0,
  `fk_account` varchar(30) NOT NULL,
  `fk_address` int(11) DEFAULT NULL,
  `fk_status` int(11) NOT NULL DEFAULT 1,
  `fk_discount` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `orders`
--

INSERT INTO `orders` (`order_id`, `creation_date`, `completion_date`, `price`, `comment`, `parcel_price`, `fk_account`, `fk_address`, `fk_status`, `fk_discount`) VALUES
(1, '2023-11-03 14:43:50', '2023-11-03 13:43:33', 0, 'Užsakymas kuriamas', 0, 'JonasPonas', NULL, 1, NULL),
(2, '2023-12-13 15:21:44', '2023-12-15 15:21:44', 200, 'Great order', 55, 'dovyd', 1, 6, NULL),
(3, '2023-12-14 15:24:16', '2023-12-16 15:24:16', 10, 'very good', 1, 'dovyd', NULL, 1, NULL);

-- --------------------------------------------------------

--
-- Table structure for table `products`
--

CREATE TABLE `products` (
  `product_id` int(11) NOT NULL,
  `name` varchar(100) NOT NULL,
  `price` float NOT NULL,
  `stock` int(11) NOT NULL,
  `description` text NOT NULL,
  `release_date` date NOT NULL,
  `being_sold` tinyint(1) NOT NULL,
  `fk_game_type` int(11) NOT NULL,
  `fk_developer` int(11) NOT NULL,
  `fk_account` varchar(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `products`
--

INSERT INTO `products` (`product_id`, `name`, `price`, `stock`, `description`, `release_date`, `being_sold`, `fk_game_type`, `fk_developer`, `fk_account`) VALUES
(1, 'Cyberpunk 2077', 59.99, 10, 'test', '2020-12-10', 1, 2, 1, 'JonasPonas'),
(2, 'Cyberpunk 2077: Phantom Liberty', 29.99, 5, 'test', '2023-09-26', 0, 2, 1, 'JonasPonas');

-- --------------------------------------------------------

--
-- Table structure for table `product_genres`
--

CREATE TABLE `product_genres` (
  `product_genre_id` int(11) NOT NULL,
  `fk_genre` int(11) NOT NULL,
  `fk_product` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `product_genres`
--

INSERT INTO `product_genres` (`product_genre_id`, `fk_genre`, `fk_product`) VALUES
(1, 1, 1),
(2, 1, 2);

-- --------------------------------------------------------

--
-- Table structure for table `statuses`
--

CREATE TABLE `statuses` (
  `status_id` int(11) NOT NULL,
  `name` varchar(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `statuses`
--

INSERT INTO `statuses` (`status_id`, `name`) VALUES
(1, 'Užsakymas kuriamas'),
(2, 'Užsakymas neapmokėtas'),
(3, 'Užsakymas apmokėtas'),
(4, 'Užsakymas apdorojamas'),
(5, 'Užsakymas išsiųstas'),
(6, 'Užsakymas užbaigtas');

-- --------------------------------------------------------

--
-- Table structure for table `user_genres`
--

CREATE TABLE `user_genres` (
  `user_genre_id` int(11) NOT NULL,
  `fk_genre` int(11) NOT NULL,
  `fk_account` varchar(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `user_genres`
--

INSERT INTO `user_genres` (`user_genre_id`, `fk_genre`, `fk_account`) VALUES
(1, 1, 'JonasPonas');

-- --------------------------------------------------------

--
-- Table structure for table `user_types`
--

CREATE TABLE `user_types` (
  `type_id` int(11) NOT NULL,
  `name` varchar(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `user_types`
--

INSERT INTO `user_types` (`type_id`, `name`) VALUES
(1, 'klientas'),
(2, 'pardavėjas'),
(3, 'administratorius');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `accounts`
--
ALTER TABLE `accounts`
  ADD PRIMARY KEY (`username`),
  ADD KEY `account-loyalty_constraint` (`fk_loyalty_tier`),
  ADD KEY `account-type_constraint` (`fk_user_type`);

--
-- Indexes for table `addresses`
--
ALTER TABLE `addresses`
  ADD PRIMARY KEY (`address_id`),
  ADD KEY `address-account_constraint` (`fk_account`);

--
-- Indexes for table `carts`
--
ALTER TABLE `carts`
  ADD PRIMARY KEY (`cart_id`),
  ADD KEY `cart-order_constraint` (`fk_order`),
  ADD KEY `cart-product_constraint` (`fk_product`);

--
-- Indexes for table `comments`
--
ALTER TABLE `comments`
  ADD PRIMARY KEY (`comment_id`),
  ADD KEY `comment-account_constraint` (`fk_account`),
  ADD KEY `comment-feedback_constraint` (`fk_feedback`);

--
-- Indexes for table `developers`
--
ALTER TABLE `developers`
  ADD PRIMARY KEY (`developer_id`);

--
-- Indexes for table `discounts`
--
ALTER TABLE `discounts`
  ADD PRIMARY KEY (`discount_id`),
  ADD KEY `discount-account_constraint` (`fk_account`);

--
-- Indexes for table `feedback`
--
ALTER TABLE `feedback`
  ADD PRIMARY KEY (`feedback_id`),
  ADD KEY `feedback-account_constraint` (`fk_account`),
  ADD KEY `feedback-product_constraint` (`fk_product`);

--
-- Indexes for table `game_types`
--
ALTER TABLE `game_types`
  ADD PRIMARY KEY (`game_type_id`);

--
-- Indexes for table `genres`
--
ALTER TABLE `genres`
  ADD PRIMARY KEY (`genre_id`);

--
-- Indexes for table `loyalty_tiers`
--
ALTER TABLE `loyalty_tiers`
  ADD PRIMARY KEY (`tier_id`);

--
-- Indexes for table `orders`
--
ALTER TABLE `orders`
  ADD PRIMARY KEY (`order_id`),
  ADD KEY `order-account_constraint` (`fk_account`),
  ADD KEY `order-discount_constraint` (`fk_discount`),
  ADD KEY `order-address_constraint` (`fk_address`),
  ADD KEY `order-status_constraint` (`fk_status`);

--
-- Indexes for table `products`
--
ALTER TABLE `products`
  ADD PRIMARY KEY (`product_id`),
  ADD KEY `product-account_constraint` (`fk_account`),
  ADD KEY `product-game-type_constraint` (`fk_game_type`),
  ADD KEY `product-developer_constraint` (`fk_developer`);

--
-- Indexes for table `product_genres`
--
ALTER TABLE `product_genres`
  ADD PRIMARY KEY (`product_genre_id`),
  ADD KEY `product_genres-genre_constraint` (`fk_genre`),
  ADD KEY `product_genres-product_constraint` (`fk_product`);

--
-- Indexes for table `statuses`
--
ALTER TABLE `statuses`
  ADD PRIMARY KEY (`status_id`);

--
-- Indexes for table `user_genres`
--
ALTER TABLE `user_genres`
  ADD PRIMARY KEY (`user_genre_id`),
  ADD KEY `user_genres-genre_constraint` (`fk_genre`),
  ADD KEY `user_genres-account_constraint` (`fk_account`);

--
-- Indexes for table `user_types`
--
ALTER TABLE `user_types`
  ADD PRIMARY KEY (`type_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `addresses`
--
ALTER TABLE `addresses`
  MODIFY `address_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `carts`
--
ALTER TABLE `carts`
  MODIFY `cart_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `comments`
--
ALTER TABLE `comments`
  MODIFY `comment_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `developers`
--
ALTER TABLE `developers`
  MODIFY `developer_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `discounts`
--
ALTER TABLE `discounts`
  MODIFY `discount_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `feedback`
--
ALTER TABLE `feedback`
  MODIFY `feedback_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `game_types`
--
ALTER TABLE `game_types`
  MODIFY `game_type_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `genres`
--
ALTER TABLE `genres`
  MODIFY `genre_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `loyalty_tiers`
--
ALTER TABLE `loyalty_tiers`
  MODIFY `tier_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `orders`
--
ALTER TABLE `orders`
  MODIFY `order_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `products`
--
ALTER TABLE `products`
  MODIFY `product_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `product_genres`
--
ALTER TABLE `product_genres`
  MODIFY `product_genre_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `statuses`
--
ALTER TABLE `statuses`
  MODIFY `status_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT for table `user_genres`
--
ALTER TABLE `user_genres`
  MODIFY `user_genre_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `user_types`
--
ALTER TABLE `user_types`
  MODIFY `type_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `accounts`
--
ALTER TABLE `accounts`
  ADD CONSTRAINT `account-loyalty_constraint` FOREIGN KEY (`fk_loyalty_tier`) REFERENCES `loyalty_tiers` (`tier_id`),
  ADD CONSTRAINT `account-type_constraint` FOREIGN KEY (`fk_user_type`) REFERENCES `user_types` (`type_id`);

--
-- Constraints for table `addresses`
--
ALTER TABLE `addresses`
  ADD CONSTRAINT `address-account_constraint` FOREIGN KEY (`fk_account`) REFERENCES `accounts` (`username`) ON DELETE CASCADE;

--
-- Constraints for table `carts`
--
ALTER TABLE `carts`
  ADD CONSTRAINT `cart-order_constraint` FOREIGN KEY (`fk_order`) REFERENCES `orders` (`order_id`) ON DELETE CASCADE,
  ADD CONSTRAINT `cart-product_constraint` FOREIGN KEY (`fk_product`) REFERENCES `products` (`product_id`) ON DELETE CASCADE;

--
-- Constraints for table `comments`
--
ALTER TABLE `comments`
  ADD CONSTRAINT `comment-account_constraint` FOREIGN KEY (`fk_account`) REFERENCES `accounts` (`username`) ON DELETE SET NULL,
  ADD CONSTRAINT `comment-feedback_constraint` FOREIGN KEY (`fk_feedback`) REFERENCES `feedback` (`feedback_id`) ON DELETE CASCADE;

--
-- Constraints for table `discounts`
--
ALTER TABLE `discounts`
  ADD CONSTRAINT `discount-account_constraint` FOREIGN KEY (`fk_account`) REFERENCES `accounts` (`username`) ON DELETE CASCADE;

--
-- Constraints for table `feedback`
--
ALTER TABLE `feedback`
  ADD CONSTRAINT `feedback-account_constraint` FOREIGN KEY (`fk_account`) REFERENCES `accounts` (`username`) ON DELETE SET NULL,
  ADD CONSTRAINT `feedback-product_constraint` FOREIGN KEY (`fk_product`) REFERENCES `products` (`product_id`) ON DELETE CASCADE;

--
-- Constraints for table `orders`
--
ALTER TABLE `orders`
  ADD CONSTRAINT `order-account_constraint` FOREIGN KEY (`fk_account`) REFERENCES `accounts` (`username`),
  ADD CONSTRAINT `order-address_constraint` FOREIGN KEY (`fk_address`) REFERENCES `addresses` (`address_id`) ON DELETE SET NULL,
  ADD CONSTRAINT `order-discount_constraint` FOREIGN KEY (`fk_discount`) REFERENCES `discounts` (`discount_id`) ON DELETE SET NULL,
  ADD CONSTRAINT `order-status_constraint` FOREIGN KEY (`fk_status`) REFERENCES `statuses` (`status_id`);

--
-- Constraints for table `products`
--
ALTER TABLE `products`
  ADD CONSTRAINT `product-account_constraint` FOREIGN KEY (`fk_account`) REFERENCES `accounts` (`username`) ON DELETE CASCADE,
  ADD CONSTRAINT `product-developer_constraint` FOREIGN KEY (`fk_developer`) REFERENCES `developers` (`developer_id`),
  ADD CONSTRAINT `product-game-type_constraint` FOREIGN KEY (`fk_game_type`) REFERENCES `game_types` (`game_type_id`);

--
-- Constraints for table `product_genres`
--
ALTER TABLE `product_genres`
  ADD CONSTRAINT `product_genres-genre_constraint` FOREIGN KEY (`fk_genre`) REFERENCES `genres` (`genre_id`),
  ADD CONSTRAINT `product_genres-product_constraint` FOREIGN KEY (`fk_product`) REFERENCES `products` (`product_id`);

--
-- Constraints for table `user_genres`
--
ALTER TABLE `user_genres`
  ADD CONSTRAINT `user_genres-account_constraint` FOREIGN KEY (`fk_account`) REFERENCES `accounts` (`username`),
  ADD CONSTRAINT `user_genres-genre_constraint` FOREIGN KEY (`fk_genre`) REFERENCES `genres` (`genre_id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;