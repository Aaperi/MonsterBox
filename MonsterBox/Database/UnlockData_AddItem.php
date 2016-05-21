<?php
      // Function: Adds requested item to all tables and to the text file containing
      // reference to all item names.

      // The item name given can't have same name with pre-existing items.
      

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
        $item_next  = mysqli_real_escape_string($connection, $_POST['item']); // name of the item being added
      }
      else
      {
       echo "FAILURE: No POST";
       exit();
      }

      // script start
      // reading the text file that contains item names
      $file = "itemName.txt";
      $itemText = file_get_contents($file);
      $items = explode(":",$itemText);

      // checks that the text file can be found and isn't empty
      if (empty($itemText))
      {
        die ("ERROR: File error. Text file not found.");
      }

      // check that the item given is not in the text file already (duplicates are problematic)
      foreach($items as $value)
      {
        if ($value == $item_next)
        {
          die ("ERROR: Given item is already in the game.");
        }
      }

      // searching for mastertable and retrieving data
      $select = "SELECT * FROM mastertable
                WHERE identification=1";
      $select_query = mysqli_query($connection, $select);
      if (!$select_query)
      {
        die("ERROR: mastertable doesn't exist");
      }
      $usercount = mysqli_fetch_array(mysqli_query($connection,$select))['usercount'];

      // inserting the item row to each table
      for ($i = 1; $i < ($usercount+1); $i++)
      {
        $forInsert = "INSERT INTO Unlock_ID_$i (itemName, unlocked)
                      VALUES ('$item_next','0')";
        if (mysqli_query($connection, $forInsert))
        {
          echo "SUCCESS: $i/$usercount done". '<br />';
        }
        else {
          echo ("ERROR?: Inserting into Unlock_ID_$i failed.". '<br />' . mysqli_error($connection) . '<br />');
        }
      }
      echo "SUCCESS: All tables should be updated with the requested item. (Item=$item_next)";

      // finally, updating the item name to the text file
      $itemText .= ":$item_next";
      file_put_contents($file, $itemText);

      mysqli_close($connection);
  ?>
