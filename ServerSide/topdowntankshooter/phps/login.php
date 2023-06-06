<?php
require "work_with_db.php";


$con = mysqli_connect('localhost', 'root', 'root', 'topdowntankshooter');

if (mysqli_connect_errno()) {
    echo "1 Connection failed"; // swap for connection failed
    exit();
}

$nickname = $_POST["nickname"];
$password = $_POST["password"];

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