const apiUrl = "http://127.0.0.1:8001/acteurs";
let isEditing = false;
let editId = null;

document.addEventListener("DOMContentLoaded", () => {
    fetchActeurs();

    document.getElementById("acteur-form").addEventListener("submit", (e) => {
        e.preventDefault();
        isEditing ? updateActeur() : addActeur();
    });

    document.getElementById("cancel-edit").addEventListener("click", () => {
        resetForm();
    });

    // recherche par {id}
    document.getElementById("search-form").addEventListener("submit", (e) => {
        e.preventDefault();
        const id = document.getElementById("search-id").value;
        if (!id) return;

        fetch(`${apiUrl}/${id}`)
            .then(res => {
                if (!res.ok) throw new Error("Acteur non trouvé");
                return res.json();
            })
            .then(acteur => {
                const container = document.getElementById("acteurs-container");
                container.innerHTML = ""; // Nettoyer la grille
                container.appendChild(renderActeurCard(acteur));
            })
            .catch(err => alert("Erreur : " + err.message));
    });
});

function fetchActeurs() {
    fetch(apiUrl)
        .then(res => res.json())
        .then(data => {
            const container = document.getElementById("acteurs-container");
            container.innerHTML = "";
            data.forEach(acteur => {
                container.appendChild(renderActeurCard(acteur));
            });
        })
        .catch(err => alert("Erreur de chargement : " + err.message));
}

function renderActeurCard(acteur) {
    const div = document.createElement("div");
    div.className = "acteur";
    div.innerHTML = `
        <h3>${acteur.name}</h3>
        <p>${acteur.bio}</p>
        <img src="${acteur.picture}" alt="${acteur.name}">
        <button onclick="prepareEdit(${acteur.id}, '${escapeHtml(acteur.name)}', '${escapeHtml(acteur.bio)}', '${acteur.picture}')">Modifier</button>
        <button onclick="deleteActeur(${acteur.id})" style="background:#f44336;">Supprimer</button>
    `;
    return div;
}

function addActeur() {
    const name = document.getElementById("name").value;
    const bio = document.getElementById("bio").value;
    const picture = document.getElementById("picture").value;
    const id = Math.floor(Math.random() * 100000); // temporaire

    fetch(apiUrl + "/", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ id, name, bio, picture })
    })
        .then(res => {
            if (!res.ok) throw new Error("Erreur lors de l'ajout");
            return res.json();
        })
        .then(() => {
            resetForm();
            fetchActeurs();
        })
        .catch(err => alert("Ajout échoué : " + err.message));
}

function prepareEdit(id, name, bio, picture) {
    document.getElementById("name").value = name;
    document.getElementById("bio").value = bio;
    document.getElementById("picture").value = picture;
    editId = id;
    isEditing = true;
}

function updateActeur() {
    const name = document.getElementById("name").value;
    const bio = document.getElementById("bio").value;
    const picture = document.getElementById("picture").value;

    fetch(`${apiUrl}/${editId}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ name, bio, picture })
    })
        .then(res => {
            if (!res.ok) throw new Error("Erreur lors de la modification");
            return res.json();
        })
        .then(() => {
            resetForm();
            fetchActeurs();
        })
        .catch(err => alert("Modification échouée : " + err.message));
}

function deleteActeur(id) {
    if (!confirm("Confirmer la suppression ?")) return;

    fetch(`${apiUrl}/${id}`, { method: "DELETE" })
        .then(res => {
            if (!res.ok) throw new Error("Erreur lors de la suppression");
            return res.json();
        })
        .then(() => fetchActeurs())
        .catch(err => alert("Suppression échouée : " + err.message));
}

function resetForm() {
    document.getElementById("acteur-form").reset();
    editId = null;
    isEditing = false;
}

// 🔐 Protection contre injection HTML
function escapeHtml(text) {
    return text.replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/'/g, "&#39;");
}
