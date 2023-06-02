<?php
$con = mysqli_connect('localhost', 'root', 'root', 'topdowntankshooter');

if (mysqli_connect_errno()) {
    echo "1 Connection failed";
    exit();
}
$nickname = $_POST["nickname"];
$queryText = "SELECT tank_main_part.name, tank_main_part.acceleration, tank_main_part.max_speed, tank_main_part.angular_speed, tank_main_part.durability, tank_main_part.ammo_storage, tank_main_part.turret_placement_x, tank_main_part.turret_placement_y, tank_main_part_sprites.sprite 
              FROM selected_parts
              INNER join users on selected_parts.user_id = users.id
              INNER join tank_main_part on selected_parts.selected_main_part = tank_main_part.id
              INNER join tank_main_part_sprites on tank_main_part.id = tank_main_part_sprites.main_part_id
              where nickname = '" . $nickname . "';";

$result = mysqli_query($con, $queryText);
$rows_amt = mysqli_num_rows($result);
if ($rows_amt == 0)
    die("User did not select any main parts");
if ($rows_amt > 1) 
{
    $rows_amt = mysqli_num_rows($result);
    $rowNumb = rand(0, $rows_amt - 1);
    $counter = 0;

    for ($i = 0; $i < $rows_amt; $i++)
    {
        $rowInfo = mysqli_fetch_assoc($result);
        if ($i == $rowNumb) 
        {
            $rowInfo["sprite"] = base64_encode($rowInfo["sprite"]);
            die ("0" . implode(",", $rowInfo));
        }
    }
    die("Error occured while trying to get random row");
} 
else 
{
    $result = mysqli_fetch_assoc($result);
    $result["sprite"] = base64_encode($result["sprite"]);
    die("0" . implode(",", $result));
}
?>