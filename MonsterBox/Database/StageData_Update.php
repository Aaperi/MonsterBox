<?php
        // FUNCTION: Updates the given stage data (row) with given data.
        // Additionally, always checks which of the data at hand is greater,
        // effectively keeping highscores.

        $server   = "localhost";
        $username = "root";
        $password = "";

        $database = "PlayerData";

        $connection = mysqli_connect($server, $username, $password, $database) or die("Cant connect into database");
        mysqli_select_db( $connection, $database) or  die("Cant connect into database");
        if($connection === false)
        {
          die("ERROR: Could not connect.". '<br />' . mysqli_connect_error());
        }
        if ($_POST)
        {
          $tableID    = mysqli_real_escape_string($connection, $_POST['ID']);
          $table      = "ID_$tableID";
          $world      = mysqli_real_escape_string($connection, $_POST['world']);
          $stage      = mysqli_real_escape_string($connection, $_POST['stage']);
          $stars      = mysqli_real_escape_string($connection, $_POST['stars']);
          $highscore  = mysqli_real_escape_string($connection, $_POST['score']);
          $bestTime   = mysqli_real_escape_string($connection, $_POST['time']);
          $chests     = mysqli_real_escape_string($connection, $_POST['chests']);
          $shards     = mysqli_real_escape_string($connection, $_POST['shards']);
        }
        else
        {
         echo "FAILURE: No POST";
         exit();
        }

        // script start
        $select = "SELECT * FROM $table
                  WHERE world=$world
                  AND stage=$stage";
        $rowSELECT = mysqli_query($connection, $select);
        // check for table and row existence
        if (!$rowSELECT)
        {
          die("FAILURE: Requested stage (row) not found". '<br />' . mysqli_error($connection));
        }
        // row exists -> store values in the row into variables
        else {
          $rowStars = mysqli_fetch_array($rowSELECT)['stars'];
          $rowHighscore = mysqli_fetch_array($rowSELECT)['highscore'];
          $rowTime = mysqli_fetch_array($rowSELECT)['bestTime'];
          $rowChests = mysqli_fetch_array($rowSELECT)['loot_chests'];
          $rowShards = mysqli_fetch_array($rowSELECT)['loot_shards'];

          $rowUnlocked = mysqli_fetch_array($rowSELECT)['unlocked'];
        }

        // checking for greatest value, to update with the highest values into the row later
        if ($stars > $rowStars) {
          $newStars = $stars;
        }
        else {
          $newStars = $rowStars;
        }
        if ($highscore > $rowHighscore) {
          $newHighscore = $highscore;
        }
        else {
          $newHighscore = $rowHighscore;
        }
        if ($bestTime > $rowTime) {
          $newTime = $bestTime;
        }
        else {
          $newTime = $rowTime;
        }
        if ($chests > $rowChests) {
          $newChests = $chests;
        }
        else {
          $newChests = $rowChests;
        }
        if ($shards > $rowShards) {
          $newShards = $shards;
        }
        else {
          $newShards = $rowShards;
        }

        // update the row
        $update = "UPDATE $table
                  SET stars=$newStars, highscore=$highscore, bestTime=$newTime,
                  loot_chests=$newChests, loot_shards=$newShards
                  WHERE stage=$stage";
        if (mysqli_query($connection, $update))
        {
          echo "SUCCESS: Update successful";
        }
        else {
          die("ERROR: Update failure". '<br />' . mysqli_error($connection));
        }

   mysqli_close($connection);
?>
