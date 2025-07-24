<?php
$API_URL = "http://127.0.0.1:8001/acteurs";

function get_acteurs() {
    global $API_URL;
    $res = @file_get_contents($API_URL);
    return $res ? json_decode($res, true) : [];
}

function get_acteur_by_id($id) {
    global $API_URL;
    $res = @file_get_contents("$API_URL/$id");
    return $res ? json_decode($res, true) : null;
}

function delete_acteur($id) {
    global $API_URL;
    $context = stream_context_create(['http' => ['method' => 'DELETE']]);
    @file_get_contents("$API_URL/$id", false, $context);
}

function add_acteur($data) {
    global $API_URL;
    $options = [
        'http' => [
            'method' => 'POST',
            'header' => "Content-Type: application/json",
            'content' => json_encode($data)
        ]
    ];
    @file_get_contents("$API_URL/", false, stream_context_create($options));
}

function update_acteur($id, $data) {
    global $API_URL;
    $options = [
        'http' => [
            'method' => 'PUT',
            'header' => "Content-Type: application/json",
            'content' => json_encode($data)
        ]
    ];
    @file_get_contents("$API_URL/$id", false, stream_context_create($options));
}
?>
