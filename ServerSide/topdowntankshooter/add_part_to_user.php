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
    $table_name = "users_main_parts";
else
    $table_name = "users_turrets";

$queryText = "Insert INTO " . $table_name . "
                values((Select id from users where nickname = '" . $nickname . "')," . $id . ");";
mysqli_query($con, $queryText) or die("4 " . $queryText);
echo("0");
?>