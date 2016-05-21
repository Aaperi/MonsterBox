<?php
        // once run, creates new table set to "PlayerData" if one is not in place already.
        // ID_[#] and Unlock_ID_[#], where # = number
        // Also creates empty rows for each stage.
        // This script works with numbers from mastertable, so refer to that also.
        // At the end of the script, the new values are updated back to mastertable,
        // so that same IDnumber tables aren't created.

        // to get what ID the php-script used in the table creation:
        // use the "-" mark to split the echo, and use the second member of the array

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

        // script start (no _POST required)
        // reads text file containing current item names
        $file = "itemName.txt";
        $itemText = file_get_contents($file);
        $items = explode(":",$itemText);

        // searches for mastertable and finds data
        $masterSelect = "SELECT * FROM mastertable
                        WHERE identification=1";
        $masterquery  = mysqli_query($connection, $masterSelect);
        if (!$masterquery)
        {
          die("ERROR: mastertable not found");
        }
        $tableID      = mysqli_fetch_array(mysqli_query($connection, $masterSelect))['nextID'];
        $userCount    = mysqli_fetch_array(mysqli_query($connection, $masterSelect))['usercount'];
        $worldCount    = mysqli_fetch_array(mysqli_query($connection, $masterSelect))['worldCount'];
        $stageArray    = mysqli_fetch_array(mysqli_query($connection, $masterSelect))['stageArray'];
        $table        = "ID_$tableID"; // stage table
        $table_unlock = "Unlock_ID_$tableID"; // unlock table (duh)
        $stages = explode(":", $stageArray);

        if (empty($tableID))
        {
          die("tableID=$tableID");
        }

        // checks that there are no tables that the script is trying to create
        $checkQuery_stage = mysqli_query($connection, "SELECT stage FROM $table");
        $checkQuery_unlock = mysqli_query($connection, "SELECT stage FROM $table_unlock");

        // unlocked is 0 or 1
        // chould check if DECIMAL works better that FLOAT
        $create_table_stage =
        "CREATE TABLE $table
        (
          world INT NOT NULL,
          stage INT NOT NULL,
          unlocked INT NOT NULL,
          stars INT NOT NULL,
          highscore FLOAT NOT NULL,
          bestTime FLOAT NOT NULL,
          loot_chests INT NOT NULL,
          loot_shards INT NOT NULL
        )";
        $create_tbl_stage = mysqli_query($connection, $create_table_stage);

        // one row for each item, so adding more of them in the future is less painful
        $create_table_unlock =
        "CREATE TABLE $table_unlock
        (
          itemName VARCHAR(30) NOT NULL,
          unlocked INT NOT NULL
        )";
        $create_tbl_unlock = mysqli_query($connection, $create_table_unlock);

        // stage table creation starts here
        // first, checks that there is no table by the name it's trying to use
        if (empty($checkQuery_stage))
        {
          // then actually running the query to create the table
          if ($create_tbl_stage) {
	           echo "Table $table has been created". '<br />';
          }
          else {
           echo "ERROR: error in $table creation". '<br />';
           echo mysqli_error($connection) . '<br />';
          }

          // next inserting empty rows for the other script to update later
          // for each world, it creates enough stage rows according to the stageArray in mastertable
          for ($i1=0; $i1<$worldCount; $i1++)
          {
            for ($i2=1; $i2 < ($stages[$i1]+1); $i2++)
            {
              $noReasonVariable1 = ($i1+1);
              $insert   = "INSERT INTO $table (world, stage, unlocked, stars, highscore,
                                               bestTime, loot_chests, loot_shards)
                          VALUES ('$noReasonVariable1', '$i2', '0', '0', '0','0','0','0')";
              if(mysqli_query($connection, $insert))
              {
                echo "Empty row for Stage " .($i1+1). "-$i2 added successfully.". '<br />';
              }
              else
              {
                die("ERROR: Could not able to execute $insert.". '<br />' . mysqli_error($connection));
              }
            }
          }
          echo "SUCCESS: Full initialization of the requested table was successful
                (player stage data)". '<br />';
        }
        else {
          die("Table $table already exists");
        }

        // unlock table creation starts here
        // first, checks that there is no table by the name it's trying to use
        if (empty($checkQuery_unlock))
        {
          // then actually running the query to create the table
          if ($create_tbl_unlock) {
             echo "Table $table_unlock has been created". '<br />';
          }
          else {
           echo "error in $table_unlock creation". '<br />';
           echo mysqli_error($connection) . '<br />';
          }

          // next inserting empty rows for the other script to update later
          for ($i=0; $i<count($items); $i++)
          {
            $insert_unlock   = "INSERT INTO $table_unlock (itemName, unlocked)
                                VALUES ('$items[$i]', '0')";
            if(mysqli_query($connection, $insert_unlock))
            {
              echo "Empty row for $items[$i] added successfully.". '<br />';
            }
            else
            {
              die("ERROR: Could not execute $insert_unlock.". '<br />' . mysqli_error($connection));
            }
          }
          echo "SUCCESS: Full initialization of the requested table was successful
                (player unlock data)". '<br />';
        }
        else {
          die("Table $table_unlock already exists");
        }

        // lastly, updates the mastertable with new values so that this script
        // won't try to create same tables over and over again
        $newNextID = $tableID+1;
        $newUsercount = $userCount+1;
        $masterIDupdate = "UPDATE mastertable
                          SET nextID=$newNextID,
                          usercount=$newUsercount";
        mysqli_query($connection, $masterIDupdate);
        echo "-$tableID";
    mysqli_close($connection);
?>
