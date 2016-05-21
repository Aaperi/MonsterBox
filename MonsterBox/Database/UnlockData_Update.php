<?php
      // function: updates given ID's given item to "unlocked" -status
      // one item per script call

      // Naturally, both the item name given and the ID given must already exist.
      // Make sure you write them correctly upon making the WWW requests!

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
        $table      = "Unlock_ID_$tableID";
        $item       = mysqli_real_escape_string($connection, $_POST['item']); // name of the item
      }
      else
      {
       echo "FAILURE: No POST";
       exit();
      }

      // script start

      // check if requested item update is even in the game
      $file = "itemName.txt";
      $itemText = file_get_contents($file);
      $items = explode(":",$itemText);

      if (empty($itemText))
      {
        die ("ERROR: File error. Reference text file not found.");
      }

      // check that the item given is not in the text file already (duplicates are problematic)
      $hiddenValue = 0;
      foreach($items as $value)
      {
        if ($value == $item)
        {
          echo "Item match found.". '<br />';
          $hiddenValue = 1;
        }
      }

      if ($hiddenValue == 0)
      {
        die ("FAILURE: Item match not found for $item");
      }


      // basically same as StageData_Unlock, refer to that if there's something strange
      $select = "SELECT * FROM $table
                WHERE itemName='$item'";

      $update = "UPDATE $table
                SET unlocked=1
                WHERE itemName='$item'";

      if (!mysqli_query($connection, $select)) // can't use empty() since itemNO will be "0" in typical situation
      {
        die ("ERROR: Selection unsuccessful. (Table/Row doesn't exist?)". '<br />' . mysqli_error($connection));
      }
      elseif (mysqli_fetch_array(mysqli_query($connection, $select))['unlocked'] == 1)
      {
        die ("FAILURE: Item $item already unlocked.");
      }
      else {
        if (mysqli_query($connection, $update))
        {
          echo "SUCCESS: Item $item unlocked (player $tableID)";
        }
        else {
          die("ERROR: Item number $item unlock failed.". '<br />' . mysqli_error($connection));
        }
      }
  mysqli_close($connection);
?>
