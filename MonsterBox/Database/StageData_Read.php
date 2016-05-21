<?php
          // FUNCTION: Reads given ID's Stage data (all of it) and
          //  returns a string that the C# script can use.
          // Format: One stage per row, values split by ":".

          $server   = "localhost";
          $username = "root";
          $password = "";

          $database = "PlayerData";

          $connection = mysqli_connect($server, $username, $password, $database) or die("Cant connect into database");
          mysqli_select_db( $connection, $database) or  die("Cant connect into database");
          if($connection === false)
          {
            die("ERROR: Could not connect." . '<br />' . mysqli_connect_error());
          }

          if ($_POST)
          {
            $tableID    = mysqli_real_escape_string($connection, $_POST['ID']);
            $table      = "Unlock_ID_$tableID";
          }
          else
          {
           echo "FAILURE: No POST";
           exit();
          }
          // script start

          // searching for mastertable and retrieving data
          $selectM = "SELECT * FROM mastertable
                    WHERE identification=1";
          $select_query = mysqli_query($connection, $selectM);
          if (!$select_query)
          {
            die("ERROR: mastertable doesn't exist");
          }
          $worldCount    = mysqli_fetch_array(mysqli_query($connection,$selectM))['worldCount'];
          $stageArray    = mysqli_fetch_array(mysqli_query($connection, $selectM))['stageArray'];
          $stages     = explode(":", $stageArray);


          // checks that the table actually exists
          $select = "SELECT * FROM $table
                    ORDER BY stage ASC";

          if (!mysqli_query($connection, $select))
          {
            die("ERROR: Selection unsuccessful.". '<br />' . mysqli_error($connection));
          }
          // table exists -> proceed to echo the results
          // selection is all stages per all worlds
          else {
            for ($i1=0; $i1<($worldCount); $i1++)
            {
              for ($i2=1; $i2<($stages[$i1]+1); $i2++)
              {
                $select2 = "SELECT * FROM $table
                            WHERE world=($i1+1)
                            AND stage=$i2";

                $fetch2 = mysqli_fetch_array(mysqli_query($connection,$select2));
                echo $fetch2['unlocked']   . ":"
                   . $fetch2['stars']      . ":"
                   . $fetch2['highscore']  . ":"
                   . $fetch2['bestTime']   . ":"
                   . $fetch2['loot_chests']. ":"
                   . $fetch2['loot_shards']. ":" . '<br />';
              }
            }
            // should return each row (as in stage) in the table as their own row, but C# doesn't seem to care either way
          }
   mysqli_close($connection);
?>
