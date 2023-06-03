<?php
$con = mysqli_connect('localhost', 'root', 'root', 'topdowntankshooter');

if (mysqli_connect_errno()) {
    echo "1 Connection failed";
    exit();
}

$nickname = $_POST["nickname"];
$id = $_POST["id"];
$part_type = $_POST["part_type"];

$column_name;
if ($part_type == "MainPartData")
    $column_name = "selected_main_part";
else
    $column_name = "selected_turret_id";

$queryText = "Update selected_parts 
              SET " . $column_name . " = $id WHERE `selected_parts`.`user_id` = (Select id from users where nickname = '" . $nickname . "');";
mysqli_query($con, $queryText) or die("4 " . $queryText);
echo ("0");
?>