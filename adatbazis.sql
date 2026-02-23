-- Adatbázis
CREATE DATABASE IF NOT EXISTS autoberles
CHARACTER SET utf8mb4
COLLATE utf8mb4_hungarian_ci;

USE autoberles;

--------------------------------------------------
-- TÍPUS TÁBLÁK (ENUM helyett)
--------------------------------------------------

CREATE TABLE valto_tipusok (
    id INT PRIMARY KEY AUTO_INCREMENT,
    megnevezes VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE uzemanyag_tipusok (
    id INT PRIMARY KEY AUTO_INCREMENT,
    megnevezes VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE klima_tipusok (
    id INT PRIMARY KEY AUTO_INCREMENT,
    megnevezes VARCHAR(50) NOT NULL UNIQUE
);

--------------------------------------------------
-- TELEPHELYEK
--------------------------------------------------

CREATE TABLE telephelyek (
    id INT PRIMARY KEY,
    varos VARCHAR(100) NOT NULL,
    cim VARCHAR(255) NOT NULL,
    email VARCHAR(150) NOT NULL,
    telefonszam VARCHAR(30) NOT NULL
);

--------------------------------------------------
-- FELHASZNALOK
--------------------------------------------------

CREATE TABLE felhasznalok (
    id INT PRIMARY KEY,
    vezeteknev VARCHAR(100) NOT NULL,
    keresztnev VARCHAR(100) NOT NULL,
    email VARCHAR(150) NOT NULL UNIQUE,
    szuletesi_datum DATE NOT NULL
);

--------------------------------------------------
-- AUTOK
--------------------------------------------------

CREATE TABLE autok (
    id INT PRIMARY KEY,
    marka VARCHAR(50) NOT NULL,
    tipus VARCHAR(50) NOT NULL,
    telephely_id INT NOT NULL,
    evjarat YEAR NOT NULL,
    valto_id INT NOT NULL,
    uzemanyagtipus_id INT NOT NULL,
    ulesek_szama TINYINT NOT NULL,
    ar INT NOT NULL,
    FOREIGN KEY (telephely_id) REFERENCES telephelyek(id),
    FOREIGN KEY (valto_id) REFERENCES valto_tipusok(id),
    FOREIGN KEY (uzemanyagtipus_id) REFERENCES uzemanyag_tipusok(id)
);

--------------------------------------------------
-- EGYEB_FELSZERELTSEGEK
--------------------------------------------------

CREATE TABLE egyeb_felszereltsegek (
    id INT PRIMARY KEY,
    auto_id INT NOT NULL,
    tolatoradar BOOLEAN NOT NULL,
    klima_id INT NOT NULL,
    ulesfutes BOOLEAN NOT NULL,
    navigacio BOOLEAN NOT NULL,
    borules BOOLEAN NOT NULL,
    FOREIGN KEY (auto_id) REFERENCES autok(id),
    FOREIGN KEY (klima_id) REFERENCES klima_tipusok(id)
);

--------------------------------------------------
-- BERLESEK
--------------------------------------------------

CREATE TABLE berlesek (
    id INT PRIMARY KEY,
    auto_id INT NOT NULL,
    felhasznalo_id INT NOT NULL,
    kezdet_datum DATE NOT NULL,
    vege_datum DATE NOT NULL,
    FOREIGN KEY (auto_id) REFERENCES autok(id),
    FOREIGN KEY (felhasznalo_id) REFERENCES felhasznalok(id)
);

--------------------------------------------------
-- TÍPUS ADATOK FELTÖLTÉSE
--------------------------------------------------

INSERT INTO valto_tipusok (megnevezes) VALUES
('manuális'),
('automata');

INSERT INTO uzemanyag_tipusok (megnevezes) VALUES
('benzines'),
('dízel'),
('hibrid'),
('elektromos');

INSERT INTO klima_tipusok (megnevezes) VALUES
('manuális'),
('automata');

INSERT INTO telephelyek VALUES
(1,'Budapest','1138 Budapest, Váci út 123.','budapest@autoker.hu','+36 1 555 1234'),
(2,'Győr','9027 Győr, Szent István út 45.','gyor@autoker.hu','+36 96 555 678'),
(3,'Szeged','6724 Szeged, Kossuth Lajos sugárút 78.','szeged@autoker.hu','+36 62 555 987'),
(4,'Debrecen','4025 Debrecen, Piac utca 12.','debrecen@autoker.hu','+36 52 555 111'),
(5,'Székesfehérvár','8000 Székesfehérvár, Palotai út 33.','fehervar@autoker.hu','+36 22 555 222'),
(6,'Pécs','7621 Pécs, Rákóczi út 50.','pecs@autoker.hu','+36 72 555 333'),
(7,'Miskolc','3525 Miskolc, Széchenyi utca 88.','miskolc@autoker.hu','+36 46 555 444'),
(8,'Zalaegerszeg','8900 Zalaegerszeg, Kossuth Lajos utca 21.','zalaegerszeg@autoker.hu','+36 92 555 555');

INSERT INTO felhasznalok VALUES
(1,'Kovács','András','kovacs.andras@email.hu','1990-05-12'),
(2,'Nagy','Eszter','nagy.eszter@email.hu','1988-11-03'),
(3,'Tóth','Bence','toth.bence@email.hu','1995-07-21'),
(4,'Szabó','Lilla','szabo.lilla@email.hu','1992-02-14'),
(5,'Varga','Dávid','varga.david@email.hu','1985-09-30'),
(6,'Kiss','Anna','kiss.anna@email.hu','1998-12-08'),
(7,'Molnár','Gábor','molnar.gabor@email.hu','1993-04-17'),
(8,'Horváth','Zsófia','horvath.zsofia@email.hu','1991-06-25');

INSERT INTO autok VALUES
(1,'Toyota','Corolla',1,2021,2,3,5,8990000),
(2,'Volkswagen','Golf',2,2019,1,1,5,6590000),
(3,'BMW','320d',4,2020,2,2,5,11290000),
(4,'Ford','Focus',3,2018,1,1,5,5290000),
(5,'Audi','A4',5,2022,2,2,5,13490000),
(6,'Hyundai','Tucson',6,2021,2,3,5,10490000),
(7,'Skoda','Octavia',7,2020,1,1,5,7890000),
(8,'Suzuki','Vitara',8,2019,1,1,5,5990000),
(9,'Mercedes','C220',1,2022,2,2,5,15990000),
(10,'Kia','Sportage',4,2021,2,3,5,10990000),
(11,'Opel','Astra',6,2019,1,1,5,5490000),
(12,'Renault','Megane',3,2020,1,2,5,6390000),
(13,'Tesla','Model 3',2,2023,2,4,5,17990000),
(14,'Peugeot','3008',5,2021,2,2,5,9890000),
(15,'Nissan','Qashqai',7,2022,2,1,5,11990000),
(16,'Mazda','CX-5',8,2020,2,1,5,10490000);

INSERT INTO egyeb_felszereltsegek VALUES
(1,1,TRUE,2,TRUE,TRUE,FALSE),
(2,2,FALSE,1,FALSE,TRUE,FALSE),
(3,3,TRUE,2,TRUE,TRUE,TRUE),
(4,4,TRUE,1,FALSE,FALSE,FALSE),
(5,5,TRUE,2,TRUE,TRUE,TRUE),
(6,6,TRUE,2,TRUE,TRUE,FALSE),
(7,7,FALSE,1,FALSE,TRUE,FALSE),
(8,8,TRUE,1,FALSE,FALSE,FALSE),
(9,9,TRUE,2,TRUE,TRUE,TRUE),
(10,10,TRUE,2,TRUE,TRUE,FALSE),
(11,11,FALSE,1,FALSE,FALSE,FALSE),
(12,12,TRUE,1,FALSE,TRUE,FALSE),
(13,13,TRUE,2,TRUE,TRUE,TRUE),
(14,14,TRUE,2,TRUE,TRUE,FALSE),
(15,15,TRUE,2,TRUE,TRUE,FALSE),
(16,16,TRUE,2,TRUE,TRUE,FALSE);

INSERT INTO berlesek VALUES
(1,3,1,'2025-03-01','2025-03-05'),
(2,8,2,'2025-03-02','2025-03-04'),
(3,13,3,'2025-03-10','2025-03-15'),
(4,5,4,'2025-04-01','2025-04-07'),
(5,1,5,'2025-04-12','2025-04-18'),
(6,10,6,'2025-05-05','2025-05-09'),
(7,15,7,'2025-05-20','2025-05-25'),
(8,6,8,'2025-06-01','2025-06-03')