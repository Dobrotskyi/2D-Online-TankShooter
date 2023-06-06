<?php
$con = mysqli_connect('localhost', 'root', 'root', 'topdowntankshooter');

if (mysqli_connect_errno()) {
    echo "1 Connection failed";
    exit();
}

$nickname = $_POST["nickname"];
$id = $_POST["id"];
$part_type = $_POST["part_type"];

$table_name;

if ($part_type == "MainPartData")
    $table_name = "tank_main_part";
else
    $table_name = "tank_turrets";
    
$queryText = "Select price from " . $table_name . " where id = ". $id . " ;";
$result = mysqli_query($con, $queryText);
$result = mysqli_fetch_assoc($result);
$price = $result["price"];

$queryText = "Update users set money = money - ". $price . " where nickname = '" . $nickname ."' ;";
mysqli_query($con, $queryText) or die("4 " . $queryText);


if ($part_type == "MainPartData")
    $table_name = "users_main_parts";
else
    $table_name = "users_turrets";

$queryText = "Insert INTO " . $table_name . "
                values((Select id from users where nickname = '" . $nickname . "')," . $id . ");";
mysqli_query($con, $queryText) or die("4 " . $queryText);
echo("0");
?>