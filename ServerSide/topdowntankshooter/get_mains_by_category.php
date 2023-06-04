<?php
$con = mysqli_connect('localhost', 'root', 'root', 'topdowntankshooter');

if (mysqli_connect_errno()) {
    echo "1 Connection failed";
    exit();
}
$nickname = $_POST["nickname"];
$purchased = $_POST["purchased"];
$switch = "";

if ($purchased == "True")
    $switch = "IN";
else
    $switch = "NOT IN";

$queryText = "SELECT tank_main_part.id, tank_main_part.name, tank_main_part.acceleration, tank_main_part.max_speed, tank_main_part.angular_speed, tank_main_part.durability, tank_main_part.ammo_storage, tank_main_part.turret_placement_x, tank_main_part.turret_placement_y, ANY_VALUE(tank_main_part_sprites.sprite) as 'sprite'
              FROM tank_main_part
              INNER join tank_main_part_sprites on tank_main_part.id = tank_main_part_sprites.main_part_id
              where tank_main_part.id " . $switch . " (Select tank_main_part.id 
                                                from users_main_parts
                                            inner join tank_main_part on users_main_parts.main_part_id = tank_main_part.id
                                            INNER join users on users_main_parts.user_id = users.id
                                            where users.nickname = '" . $nickname . "')
                                            GROUP by tank_main_part.id;";

$result = mysqli_query($con, $queryText);
$rows_amt = mysqli_num_rows($result);

if ($rows_amt == 0)
    die("0" . "All items were bought");
else {
    echo ("0");
    for ($i = 0; $i < mysqli_num_rows($result); $i++) {
        $rowInfo = mysqli_fetch_assoc($result);
        $rowInfo["sprite"] = base64_encode($rowInfo["sprite"]);
        echo (implode(",", $rowInfo) . ",\t");
        if ($i != $rows_amt - 1)
            echo (",");
    }
    exit();
}
?>