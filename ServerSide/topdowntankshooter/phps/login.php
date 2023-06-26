<?php
require "work_with_db.php";


$con = mysqli_connect('localhost', 'root', 'root', 'topdowntankshooter');

if (mysqli_connect_errno()) {
    echo "1 Connection failed"; // swap for connection failed
    exit();
}

$nickname = $_POST["nickname"];
$nickname = mysqli_real_escape_string($con, $nickname);
$nicknameclean = filter_var($nickname, FILTER_SANITIZE_STRING, FILTER_FLAG_STRIP_LOW | FILTER_FLAG_STRIP_HIGH);
if($nickname != $nicknameclean)
    die("Inappropriate nickname");

$password = $_POST["password"];
$password = mysqli_real_escape_string($con, $password);
$passwordclean = filter_var($password, FILTER_SANITIZE_STRING, FILTER_FLAG_STRIP_LOW | FILTER_FLAG_STRIP_HIGH);

if($password != $passwordclean)
    die("Inappropriate password");

if (nicknameIsUsed($con, $nickname) == false) {
    echo "No such user";
    exit();
}

$result = tryLogin($con, $nickname, $password);
    
if ($result != "No such user" and $result != "Incorrect password") {
    echo "0\t" . $result["money"];
    exit();
} else {
    die($result);
}
?>