<?php
$con = mysqli_connect('localhost', 'root', 'root', 'topdowntankshooter');

if (mysqli_connect_errno()) {
    echo "1 Connection failed"; // swap for connection failed
    exit();
}

$nickname = $_POST["nickname"];

$queryText = "Select money from users where nickname = '" . $nickname . "';";
$result = mysqli_query($con, $queryText) or die("4 " . $queryText);
$result = mysqli_fetch_assoc($result);

$money = $result["money"];
die ("0" . $money);
?>