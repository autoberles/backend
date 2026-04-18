-- Database

CREATE DATABASE IF NOT EXISTS car_rental
CHARACTER SET utf8mb4
COLLATE utf8mb4_hungarian_ci;

USE car_rental;

-- Transmission types
CREATE TABLE transmission_types (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(50) NOT NULL UNIQUE
);

-- Fuel types
CREATE TABLE fuel_types (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(50) NOT NULL UNIQUE
);

-- Air conditioning types
CREATE TABLE air_conditioning_types (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(50) NOT NULL UNIQUE
);

-- Wheel drive types
CREATE TABLE wheel_drive_types (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(50) NOT NULL UNIQUE
);

-- Car categories
CREATE TABLE car_categories (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(50) NOT NULL UNIQUE
);

-- Branches
CREATE TABLE branches (
    id INT PRIMARY KEY AUTO_INCREMENT,
    city VARCHAR(100) NOT NULL,
    address VARCHAR(255) NOT NULL UNIQUE,
    email VARCHAR(150) NOT NULL UNIQUE,
    phone_number VARCHAR(30) NOT NULL UNIQUE
);

-- Users
CREATE TABLE users (
    id INT PRIMARY KEY AUTO_INCREMENT,
    last_name VARCHAR(100) NOT NULL,
    first_name VARCHAR(100) NOT NULL,
    email VARCHAR(150) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    reset_token_hash VARCHAR(255) NULL,
    reset_token_expiry DATETIME NULL,
    phone_number VARCHAR(30) NOT NULL UNIQUE,
    birth_date DATE NOT NULL,
    role VARCHAR (30) NOT NULL
);

-- Cars
CREATE TABLE cars (
    id INT PRIMARY KEY AUTO_INCREMENT,
    availability BOOLEAN NOT NULL,
    brand VARCHAR(50) NOT NULL,
    model VARCHAR(50) NOT NULL,
    year YEAR NOT NULL,
    color VARCHAR(50) NOT NULL,
    own_weight INT NOT NULL,
    total_weight INT NOT NULL,
    number_of_seats SMALLINT NOT NULL,
    number_of_doors SMALLINT NOT NULL,
    price INT NOT NULL,
    license_plate VARCHAR(20) NOT NULL UNIQUE,
    mileage INT NOT NULL,
    luggage_capacity INT NOT NULL,
    cubic_capacity INT NOT NULL,
    tank_capacity INT,
    battery_capacity INT,
    performance_kw INT NOT NULL,
    performance_hp INT NOT NULL,
    last_service_date DATE NOT NULL,
    inspection_expiry_date DATE NOT NULL,
    branch_id INT NOT NULL,
    transmission_id INT NOT NULL,
    fuel_type_id INT NOT NULL,
    wheel_drive_type_id INT NOT NULL,
    car_category_id INT NOT NULL,
    default_price_per_day INT NOT NULL,
    img_url VARCHAR(500) NOT NULL,
    FOREIGN KEY (branch_id) REFERENCES branches(id),
    FOREIGN KEY (transmission_id) REFERENCES transmission_types(id),
    FOREIGN KEY (fuel_type_id) REFERENCES fuel_types(id),
    FOREIGN KEY (wheel_drive_type_id) REFERENCES wheel_drive_types(id),
    FOREIGN KEY (car_category_id) REFERENCES car_categories(id)
);

-- Additional equipment
CREATE TABLE additional_equipment (
    id INT PRIMARY KEY AUTO_INCREMENT,
    car_id INT NOT NULL,
    parking_sensors BOOLEAN NOT NULL,
    air_conditioning_id INT NOT NULL,
    heated_seats BOOLEAN NOT NULL,
    navigation BOOLEAN NOT NULL,
    leather_seats BOOLEAN NOT NULL,
    tempomat BOOLEAN NOT NULL,
    FOREIGN KEY (car_id) REFERENCES cars(id),
    FOREIGN KEY (air_conditioning_id) REFERENCES air_conditioning_types(id)
);

-- Rentals
CREATE TABLE rentals (
    id INT PRIMARY KEY AUTO_INCREMENT,
    car_id INT NOT NULL,
    user_id INT NOT NULL,
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    return_date DATE,
    damage TEXT,
    damage_cost INT,
    full_price INT NOT NULL,
    FOREIGN KEY (car_id) REFERENCES cars(id),
    FOREIGN KEY (user_id) REFERENCES users(id)
);

-- Transmission types
INSERT INTO transmission_types (name) VALUES
('automata'),
('dupla kuplungos'),
('fokozatmentes automata'),
('manuális');

-- Fuel types
INSERT INTO fuel_types (name) VALUES
('benzines'),
('dízel'),
('elektromos'),
('hibrid');

-- Air conditioning types
INSERT INTO air_conditioning_types (name) VALUES
('automata'),
('manuális');

-- Wheel drive types
INSERT INTO wheel_drive_types (name) VALUES
('elsőkerék-meghajtású'),
('hátsókerék-meghajtású'),
('összkerékmeghajtású');

-- Car categories
INSERT INTO car_categories (name) VALUES
('gazdaságos kisautó'),
('középkategóriás családi autó'),
('nagykategóriás autó'),
('sportautó'),
('városi terepjáró');

-- Branches
INSERT INTO branches VALUES
(1,'Budapest','1044 Budapest, Váci út 123.','budapest@autoker.hu','+36 15 555 1231'),
(2,'Győr','9022 Győr, Szent István út 45.','gyor@autoker.hu','+36 96 555 6781'),
(3,'Szeged','6724 Szeged, Kossuth Lajos sugárút 78.','szeged@autoker.hu','+36 62 555 9871'),
(4,'Debrecen','4026 Debrecen, Piac utca 12.','debrecen@autoker.hu','+36 52 555 1111'),
(5,'Székesfehérvár','8000 Székesfehérvár, Palotai út 33.','fehervar@autoker.hu','+36 22 555 2221'),
(6,'Pécs','7621 Pécs, Rákóczi út 50.','pecs@autoker.hu','+36 72 555 3331'),
(7,'Miskolc','3711 Miskolc, Széchenyi utca 88.','miskolc@autoker.hu','+36 46 555 4441'),
(8,'Zalaegerszeg','8900 Zalaegerszeg, Kossuth Lajos utca 21.','zalaegerszeg@autoker.hu','+36 92 555 5551');

-- Users
INSERT INTO users VALUES
(1,'Kovács','András','kovacs.andras@email.hu','$2a$11$8JvY8mQ7k0zJk6V3bYwqUeQ7YxXlKz2mFv7H9pR1nD3tWcA5sE6uK',NULL,NULL,'+36 20 111 1111','1990-05-12','customer'),
(2,'Nagy','Eszter','nagy.eszter@email.hu','$2a$11$2QkFvWm9YpT6rX1cJz8NHeL4sB7uA3dG5hK0nM2pR9tYwC6xZ8vQW',NULL,NULL,'+36 20 222 2222','1988-11-03','customer'),
(3,'Tóth','Bence','toth.bence@email.hu','$2a$11$zX9yW8vU7tS6rQ5pO4nM3lK2jI1hG0fE9dC8bA7a6Z5y4X3w2V1uT',NULL,NULL,'+36 20 333 3333','1995-07-21','customer'),
(4,'Szabó','Lilla','szabo.lilla@email.hu','$2a$11$LmN8oP7qR6sT5uV4wX3yZ2aB1cD0eF9gH8iJ7kL6mN5oP4qR3sT2u',NULL,NULL,'+36 20 444 4444','1992-02-14','customer'),
(5,'Varga','Dávid','varga.david@email.hu','$2a$11$9x8y7z6w5v4u3t2s1r0qPOnMlKjIhGfEdCbA9876543210qwertyui',NULL,NULL,'+36 20 555 5555','1985-09-30','customer'),
(6,'Kiss','Anna','kiss.anna@email.hu','$2a$11$AbCdEfGhIjKlMnOpQrStUvWxYz1234567890abcdefghijklmnopqr',NULL,NULL,'+36 20 666 6666','1998-12-08','customer'),
(7,'Molnár','Gábor','molnar.gabor@email.hu','$2a$11$XyZ123456789abcdefghijklmnopqrstuvwABCDEFGHIJKLMNOPQRS',NULL,NULL,'+36 20 777 7777','1993-04-17','customer'),
(8,'Horváth','Zsófia','horvath.zsofia@email.hu','$2a$11$QwErTyUiOpAsDfGhJkLzXcVbNm1234567890poiuytrewqlkjhgfds',NULL,NULL,'+36 20 888 8888','1991-06-25','customer'),
(9,'Nagy','Lajos','nagy.lajos@email.hu','$2a$11$MnbVcXzAsDfGhJkLqWeRtYuIoP0987654321poiuytrewqlkjhgfds',NULL,NULL,'+36 20 999 9999','1973-04-17','customer'),
(10,'Papp','Éva','papp.eva@email.hu','$2a$11$ZxCvBnMaSdFgHjKlQwErTyUiOp1234567890poiuytrewqlkjhgfdsa',NULL,NULL,'+36 30 111 1111','1996-07-08','customer'),
(11,'Szilágyi','Attila','szilagyi.attila@email.hu','$2a$11$YtReWqAsDfGhJkLzXcVbNm1234567890poiuytrewqlkjhgfdsazxcv',NULL,NULL,'+36 30 222 2222','1987-09-19','customer'),
(12,'Vass','Réka','vass.reka@email.hu','$2a$11$UuIiOoPpAaSsDdFfGgHhJjKkLlZzXxCcVvBbNnMm1234567890asdfg',NULL,NULL,'+36 30 333 3333','1992-11-22','customer'),
(13,'Tóth','Márton','toth.marton@email.hu','$2a$11$KkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz1234567890abcdefghijklmn',NULL,NULL,'+36 30 444 4444','1990-01-15','customer'),
(14,'Németh','Bianka','nemeth.bianka@email.hu','$2a$11$HgFdSaQwErTyUiOpLkJHgfdsazxcvbnm1234567890poiuytrewqlkj',NULL,NULL,'+36 30 555 5555','1998-05-30','customer'),
(15,'Kelemen','Zoltán','kelemen.zoltan@email.hu','$2a$11$PpOoIiUuYyTtRrEeWwQqAaSsDdFfGgHhJjKkLlZzXxCcVvBbNnMm123',NULL,NULL,'+36 30 666 6666','1989-08-04','customer'),
(16,'Major','Judit','major.judit@email.hu','$2a$11$AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz123',NULL,NULL,'+36 30 777 7777','1995-12-18','customer'),
(17,'Farkas','Balázs','farkas.balazs@email.hu','$2a$11$1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZabcd',NULL,NULL,'+36 30 888 8888','1994-03-12','customer');

-- Cars
INSERT INTO cars VALUES
(1,TRUE,'Toyota','Corolla',2021,'fehér',1350,1850,5,4,8990000,'AB-CD-101',45000,470,1798,50,NULL,97,132,'2025-09-15','2026-06-30',2,3,2,1,2,15000,'http://localhost:5128/cars/toyota_corolla.jpg'),
(2,TRUE,'Volkswagen','Golf',2019,'fekete',1280,1780,5,5,6590000,'EF-GH-214',80000,380,1498,50,NULL,85,116,'2024-07-10','2026-09-09',1,1,1,1,1,14000,'http://localhost:5128/cars/volkswagen_golf.jpg'),
(3,TRUE,'BMW','320d',2020,'kék',1500,2050,5,4,11290000,'IJ-KL-335',60000,480,1995,57,NULL,140,190,'2025-01-20','2026-05-09',2,2,2,2,3,20000,'http://localhost:5128/cars/bmw_320d.jpg'),
(4,TRUE,'Ford','Focus',2018,'ezüst',1320,1820,5,5,5290000,'MN-OP-442',90000,375,1496,52,NULL,92,125,'2024-05-05','2027-03-19',3,1,1,1,2,13000,'http://localhost:5128/cars/ford_focus.jpg'),
(5,FALSE,'Audi','A4',2022,'szürke',1550,2100,5,4,13490000,'QR-ST-558',30000,460,1984,54,NULL,150,204,'2026-01-18','2026-06-30',2,2,2,1,3,21000,'http://localhost:5128/cars/audi_a4.jpg'),
(6,TRUE,'Hyundai','Tucson',2021,'fehér',1650,2200,5,5,10490000,'UV-WX-671',40000,513,1598,52,13,132,179,'2025-08-12','2026-08-30',2,3,3,1,4,18000,'http://localhost:5128/cars/hyundai_tucson.jpg'),
(7,TRUE,'Skoda','Octavia',2020,'fekete',1380,1880,5,5,7890000,'YZ-AB-782',70000,600,1968,50,NULL,110,150,'2024-12-30','2026-05-05',1,1,1,1,2,15000,'http://localhost:5128/cars/skoda_octavia.jpg'),
(8,TRUE,'Suzuki','Vitara',2019,'kék',1200,1700,5,5,5990000,'CD-EF-893',85000,375,1373,47,NULL,103,140,'2024-06-15','2026-07-18',1,1,1,1,4,14000,'http://localhost:5128/cars/suzuki_vitara.jpg'),
(9,TRUE,'Mercedes','C220',2022,'ezüst',1600,2150,5,4,15990000,'GH-IJ-904',25000,455,1950,66,NULL,143,194,'2026-02-01','2026-05-30',2,4,2,2,3,23000,'http://localhost:5128/cars/mercedes_c220.jpg'),
(10,TRUE,'Kia','Sportage',2021,'szürke',1680,2230,5,5,10990000,'KL-MN-315',50000,503,1598,54,14,132,179,'2025-09-01','2026-08-31',4,2,3,1,4,18000,'http://localhost:5128/cars/kia_sportage.jpg'),
(11,TRUE,'Opel','Astra',2019,'fehér',1250,1750,5,5,5490000,'OP-QR-426',95000,370,1399,48,NULL,81,110,'2024-07-20','2026-09-01',6,1,1,1,1,13000,'http://localhost:5128/cars/opel_astra.jpg'),
(12,TRUE,'Renault','Megane',2020,'fekete',1320,1820,5,5,6390000,'ST-UV-537',75000,440,1332,50,NULL,97,132,'2025-02-10','2027-03-12',3,1,2,1,2,14000,'http://localhost:5128/cars/renault_megane.jpg'),
(13,FALSE,'Tesla','Model 3',2023,'piros',1840,2250,5,4,17990000,'WX-YZ-648',15000,425,0,NULL,60,208,283,'2026-03-01','2027-06-30',2,4,4,3,3,25000,'http://localhost:5128/cars/tesla_model3.jpg'),
(14,TRUE,'Peugeot','3008',2021,'kék',1600,2100,5,5,9890000,'AA-BB-759',40000,520,1598,53,NULL,121,165,'2025-08-20','2026-10-30',5,2,2,1,4,17000,'http://localhost:5128/cars/peugeot_3008.jpg'),
(15,TRUE,'Porsche','911 Carrera',2022,'piros',1505,1850,2,2,45990000,'GG-HH-123',10000,132,2981,NULL,70,283,385,'2026-02-15','2026-08-22',2,2,4,2,5,40000,'http://localhost:5128/cars/porsche_911carrera.jpg'),
(16,TRUE,'Nissan','Qashqai',2022,'szürke',1500,2000,5,5,11990000,'CC-DD-860',35000,504,1332,55,NULL,116,158,'2026-01-25','2026-06-26',7,2,1,1,4,18000,'http://localhost:5128/cars/nissan_qashqai.jpg'),
(17,TRUE,'Mazda','CX-5',2020,'ezüst',1620,2140,5,5,10490000,'EE-FF-971',55000,506,2488,58,NULL,143,194,'2025-03-10','2026-09-15',8,2,1,1,4,18000,'http://localhost:5128/cars/mazda_cx5.jpeg');

-- Additional Equipment
INSERT INTO additional_equipment VALUES
(1,1,TRUE,2,TRUE,TRUE,FALSE,FALSE),
(2,2,FALSE,1,FALSE,TRUE,FALSE,FALSE),
(3,3,TRUE,2,TRUE,TRUE,TRUE,TRUE),
(4,4,TRUE,1,FALSE,FALSE,FALSE,FALSE),
(5,5,TRUE,2,TRUE,TRUE,TRUE,TRUE),
(6,6,TRUE,2,TRUE,TRUE,FALSE,FALSE),
(7,7,FALSE,1,FALSE,TRUE,FALSE,FALSE),
(8,8,TRUE,1,FALSE,FALSE,FALSE,FALSE),
(9,9,TRUE,2,TRUE,TRUE,TRUE,TRUE),
(10,10,TRUE,2,TRUE,TRUE,FALSE,FALSE),
(11,11,FALSE,1,FALSE,FALSE,FALSE,FALSE),
(12,12,TRUE,1,FALSE,TRUE,FALSE,FALSE),
(13,13,TRUE,2,TRUE,TRUE,TRUE,TRUE),
(14,14,TRUE,2,TRUE,TRUE,FALSE,FALSE),
(15,15,TRUE,2,TRUE,TRUE,TRUE,TRUE),
(16,16,TRUE,2,TRUE,TRUE,FALSE,FALSE),
(17,17,FALSE,1,FALSE,TRUE,FALSE,FALSE);

-- Rentals
INSERT INTO rentals VALUES
(1,13,3,'2026-03-16','2026-03-20','2026-03-20',NULL,NULL,100000),
(2,5,4,'2026-03-17','2026-03-22','2026-03-22',NULL,NULL,126000),
(3,1,5,'2026-03-22','2026-03-28','2026-03-28',NULL,NULL,105000),
(4,10,6,'2026-03-25','2026-03-29','2026-03-29',NULL,NULL,90000),
(5,16,7,'2026-04-10','2026-04-15',NULL,NULL,NULL,108000),
(6,6,8,'2026-04-11','2026-04-13',NULL,NULL,NULL,54000),
(7,2,9,'2026-04-10','2026-04-15',NULL,NULL,NULL,84000),
(8,3,10,'2026-04-20','2026-04-25',NULL,NULL,NULL,120000),
(9,8,11,'2026-05-01','2026-05-04',NULL,NULL,NULL,84000),
(10,14,12,'2026-05-05','2026-05-10',NULL,NULL,NULL,102000),
(11,9,13,'2026-05-12','2026-05-16',NULL,NULL,NULL,115000),
(12,15,14,'2026-05-18','2026-05-20',NULL,NULL,NULL,120000),
(13,7,15,'2026-05-21','2026-05-25',NULL,NULL,NULL,75000),
(14,11,16,'2026-05-26','2026-05-30',NULL,NULL,NULL,65000);


-- Triggers

-- 1. Telephely törlése előtt ellenőrzés: csak akkor törölhető, ha nincs autó rajta
CREATE TRIGGER before_branch_delete
BEFORE DELETE ON branches
FOR EACH ROW
BEGIN
    IF EXISTS (SELECT 1 FROM cars WHERE branch_id = OLD.id) THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'A telephely nem törölhető, mert tartozik hozzá autó!';
    END IF;
END;

-- 2. Bérlés törlése előtt: csak akkor lehet törölni, ha már visszahozták az autót
CREATE TRIGGER before_rental_delete
BEFORE DELETE ON rentals
FOR EACH ROW
BEGIN
    IF OLD.return_date IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'A bérlés nem törölhető, mert még nem lett visszahozva az autó!';
    END IF;

    IF OLD.return_date > CURDATE() THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'A bérlés nem törölhető, mert a visszahozatal dátuma a jövőben van!';
    END IF;
END;