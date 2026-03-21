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



-- 2. Bérlés törlése előtt: csak akkor lehet törölni, ha már visszahozták az autót
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