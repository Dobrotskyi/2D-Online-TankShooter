<?php
$con = mysqli_connect('localhost', 'root', 'root', 'topdowntankshooter');

if (mysqli_connect_errno()) {
    echo "1 Connection failed";
    exit();
}
$nickname = $_POST["nickname"];

$queryText = "Select tank_turrets.name, tank_turrets.rotation_speed , tank_turrets.spread_x, tank_turrets.spread_y,
              tank_turrets.fire_rate, tank_turrets.shot_force, tank_turrets.durability_mult,
              tank_turrets.damage_mult, tank_turrets_sprites.sprite as 'turret_sprite',
              projectiles.name as 'projectile_name',
              projectiles.damage, projectiles.time_of_life, projectiles.ammo_cost, projectiles_sprites.sprite as 'projectile_sprite'
              from selected_parts
              inner join users on selected_parts.user_id = users.id
              inner join tank_turrets on selected_turret_id = tank_turrets.id
              inner join tank_turrets_sprites on tank_turrets.id = tank_turrets_sprites.turret_id
              inner join projectiles on tank_turrets.projectile_id = projectiles.id
              inner join projectiles_sprites on projectiles.id = projectiles_sprites.projectile_id
              where nickname = '" . $nickname . "';";

$result = mysqli_query($con, $queryText);
$rows_amt = mysqli_num_rows($result);
if ($rows_amt == 0)
    die("User did not select any turrets");
if($rows_amt > 1)
{
    $rows_amt = mysqli_num_rows($result);
    $rowNumb = rand(0, $rows_amt - 1);
    $counter = 0;

    for ($i = 0; $i < $rows_amt; $i++)
    {
        $rowInfo = mysqli_fetch_assoc($result);
        if ($i == $rowNumb) 
        {
            $rowInfo["turret_sprite"] = base64_encode($rowInfo["turret_sprite"]);
            $rowInfo["projectile_sprite"] = base64_encode($rowInfo["projectile_sprite"]);
            die ("0" . implode(",", $rowInfo));
        }
    }
    die("Error occured while trying to get random row");
}
else {
    $result = mysqli_fetch_assoc($result);
    $result["turret_sprite"] = base64_encode($result["turret_sprite"]);
    $result["projectile_sprite"] = base64_encode($result["projectile_sprite"]);
    
    die( "0" . implode(",", $result));
}
?>