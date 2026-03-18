-- 1. Telephely törlése előtt ellenőrzés: csak akkor törölhető, ha nincs autó rajta
CREATE TRIGGER car_rental.before_branch_delete
BEFORE DELETE ON car_rental.branches
FOR EACH ROW
BEGIN
    IF EXISTS (SELECT 1 FROM cars WHERE branch_id = OLD.id) THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'A telephely nem törölhető, mert tartozik hozzá autó!';
    END IF;
END;

-- 2. Új bérlés felvétele után: ha a bérlés kezdete ma van, az autó nem elérhető
CREATE TRIGGER car_rental.after_rental_insert
AFTER INSERT ON car_rental.rentals
FOR EACH ROW
BEGIN
    IF CURDATE() BETWEEN NEW.start_date AND NEW.end_date THEN
        UPDATE cars
        SET availability = FALSE
        WHERE id = NEW.car_id;
    END IF;
END;

-- 3. Bérlés frissítése után: ha visszahozta a bérlő az autót, az autó újra elérhető
CREATE TRIGGER car_rental.after_rental_update
AFTER UPDATE ON car_rental.rentals
FOR EACH ROW
BEGIN
    IF NEW.return_date IS NOT NULL THEN
        UPDATE cars
        SET availability = TRUE
        WHERE id = NEW.car_id;
    END IF;
END;

-- 4. Bérlés törlése előtt: csak akkor lehet törölni, ha már visszahozták az autót
CREATE TRIGGER car_rental.before_rental_delete
BEFORE DELETE ON car_rental.rentals
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