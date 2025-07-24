<?php
include 'fonctions.php';
ini_set('display_errors', 1);
error_reporting(E_ALL);

// Traitement POST
if ($_SERVER["REQUEST_METHOD"] === "POST") {
    if (isset($_POST['delete'])) {
        delete_acteur($_POST['delete']);
    } elseif (isset($_POST['edit_id'])) {
        update_acteur($_POST['edit_id'], [
            "name" => $_POST["name"],
            "bio" => $_POST["bio"],
            "picture" => $_POST["picture"]
        ]);
    } elseif (isset($_POST['name'], $_POST['bio'], $_POST['picture'])) {
        $new_id = rand(1000, 9999);
        add_acteur([
            "id" => $new_id,
            "name" => $_POST["name"],
            "bio" => $_POST["bio"],
            "picture" => $_POST["picture"]
        ]);
    }
    header("Location: index.php");
    exit;
}

// Recherche ?
$searched = null;
if (isset($_GET["search_id"]) && is_numeric($_GET["search_id"])) {
    $searched = get_acteur_by_id($_GET["search_id"]);
}

$acteurs = $searched ? [$searched] : get_acteurs();
?>

<!DOCTYPE html>
<html lang="fr">
<head>
    <meta charset="UTF-8">
    <title>Acteurs FastAPI PHP</title>
    <style>
        body {
            background: #111;
            color: white;
            font-family: sans-serif;
            padding: 20px;
        }
        h1 {
            color: gold;
            text-align: center;
        }
        form {
            margin: 20px auto;
            max-width: 500px;
            background: #222;
            padding: 20px;
            border-radius: 10px;
        }
        input, button {
            width: 100%;
            padding: 10px;
            margin-top: 10px;
            border-radius: 5px;
        }
        button {
            background: gold;
            color: black;
            font-weight: bold;
            cursor: pointer;
        }
        .acteur {
            background: #1a1a1a;
            margin: 15px;
            padding: 15px;
            border-radius: 10px;
            display: inline-block;
            width: 300px;
            vertical-align: top;
        }
        .acteur img {
            width: 100%;
            height: 200px;
            object-fit: cover;
            border-radius: 10px;
        }
        .acteur h3 {
            margin-top: 10px;
            color: gold;
        }
    </style>
</head>
<body>

<h1>🎬 Gestion des Acteurs (PHP + FastAPI)</h1>

<!-- Formulaire ajout / modif -->
<form method="POST">
    <input type="text" name="name" placeholder="Nom" required>
    <input type="text" name="bio" placeholder="Biographie" required>
    <input type="text" name="picture" placeholder="URL image" required>
    <button type="submit">Enregistrer</button>
</form>

<!-- Formulaire recherche -->
<form method="GET">
    <input type="number" name="search_id" placeholder="Rechercher par ID">
    <button type="submit">Rechercher</button>
    <a href="index.php"><button type="button">Réinitialiser</button></a>
</form>

<!-- Liste des acteurs -->
<?php foreach ($acteurs as $a): ?>
    <div class="acteur">
        <img src="<?= htmlspecialchars($a['picture']) ?>" alt="<?= htmlspecialchars($a['name']) ?>">
        <h3><?= htmlspecialchars($a['name']) ?></h3>
        <p><?= htmlspecialchars($a['bio']) ?></p>

        <!-- Supprimer -->
        <form method="POST">
            <input type="hidden" name="delete" value="<?= $a['id'] ?>">
            <button style="background:red; color:white;">Supprimer</button>
        </form>

        <!-- Modifier -->
        <form method="POST">
            <input type="hidden" name="edit_id" value="<?= $a['id'] ?>">
            <input type="text" name="name" value="<?= htmlspecialchars($a['name']) ?>" required>
            <input type="text" name="bio" value="<?= htmlspecialchars($a['bio']) ?>" required>
            <input type="text" name="picture" value="<?= htmlspecialchars($a['picture']) ?>" required>
            <button>Modifier</button>
        </form>
    </div>
<?php endforeach; ?>

</body>
</html>
