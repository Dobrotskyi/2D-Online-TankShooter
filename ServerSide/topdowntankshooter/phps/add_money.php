<?php
$con = mysqli_connect('localhost', 'root', 'root', 'topdowntankshooter');
if (mysqli_connect_errno()) {
    echo "1 Connection failed"; // swap for connection failed
    exit();
}

$nickname = $_POST["nickname"];
$add_money = $_POST["add_money"];

$queryText = "UPDATE users 
              SET money = money + " . $add_money .
              " WHERE nickname = '" . $nickname . "';";

mysqli_query($con, $queryText) or die("4 " . $queryText);
die("0");
?>