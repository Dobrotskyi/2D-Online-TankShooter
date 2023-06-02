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
                from tank_turrets
                INNER join tank_turrets_sprites on tank_turrets.id = tank_turrets_sprites.turret_id
                INNER JOIN projectiles on tank_turrets.projectile_id = projectiles.id
                INNER join projectiles_sprites on projectiles.id = projectiles_sprites.projectile_id
                where tank_turrets.id not IN (SELECT tank_turrets.id                              
                                                from users_turrets 
                                                INNER join tank_turrets on users_turrets.tank_turret_id = tank_turrets.id
                                                INNER join users on users_turrets.user_id = users.id
                                                WHERE users.nickname = " . $nickname .")
                                                LIMIT 1;"
?>