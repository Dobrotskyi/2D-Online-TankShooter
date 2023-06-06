<?php
$con = mysqli_connect('localhost', 'root', 'root', 'topdowntankshooter');

if (mysqli_connect_errno()) {
    echo "1 Connection failed";
    exit();
}

$nickname = $_POST["nickname"];

$queryText = "Select selected_turret_id, selected_main_part 
              from selected_parts where user_id = (Select id from users where nickname = '" . $nickname . "');";

$result = mysqli_query($con, $queryText);
if (mysqli_num_rows($result) == 0)
    die("User did not select any parts");

    $result = mysqli_fetch_assoc($result);

die("0" . implode(",", $result));
?>