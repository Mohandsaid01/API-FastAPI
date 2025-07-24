import json
from dataclasses import dataclass, asdict
from http.client import HTTPException
from fastapi import HTTPException

from fastapi import FastAPI, Path
from pydantic import BaseModel
from fastapi.middleware.cors import CORSMiddleware
# chercher une base de données
# as c'est un allias
with open("acteurs.json", "r") as f:  #ouvrir la db en mode lecture
    acteurs_list = json.load(f)  #lecture du fichier f et convertion en objet python

list_acteurs = {k + 1: v for k, v in enumerate(acteurs_list)}


#pour la faire class model il faut ajouter de la déco
@dataclass
class Acteur():
    id: int
    name: str
    bio: str
    picture: str

class ActeurUpdate(BaseModel):
    name: str
    bio: str
    picture: str

#création d'une varaible pour appler lapi
#il faut aussi l'importer

app = FastAPI()
# Ajout du middleware CORS
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],  # ou ["http://127.0.0.1:5500"]
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],)

#méthode pour sauvgarde de données
def save_to_file():
    with open("acteurs.json", "w") as f:
        json.dump(list(list_acteurs.values()), f, indent=5)


#pour faire le total des documents
@app.get("/total_acteurs")
def get_total_acteurs() -> dict:
    return {"total": len(list_acteurs)}


@app.get("/acteurs")
def get_all_acteurs() -> list[Acteur]:
    res = []
    for id in list_acteurs:
        res.append(Acteur(**list_acteurs[id]))  #conversion en Dictionnaire grace à **
    return res


@app.get("/acteurs/{id}")
def get_acteurs_by_id(id: int = Path(ge=1)) -> Acteur:
    if id not in list_acteurs:
        raise HTTPException(status_code=404, detail=f"cet acteur {id} n'existe pas")
    return Acteur(**list_acteurs[id])


@app.post("/acteurs/")
def create_acteur(acteur: Acteur) -> Acteur:
    if acteur.id in list_acteurs:
        raise HTTPException(status_code=409, detail=f"cet acteur existe déjà {acteur.id}")
    list_acteurs[acteur.id] = asdict(acteur)  #asdict trasform object en dictionnaire
    save_to_file()
    return acteur


@app.put("/acteurs/{id}")
def update_acteur(acteur: ActeurUpdate, id: int = Path(ge=1)) -> Acteur:
    if id not in list_acteurs:
        raise HTTPException(status_code=404, detail="cet acteur{id} n'existe pas")
    update_data_acteur = Acteur(id=id, name=acteur.name,bio=acteur.bio,picture=acteur.picture)
    list_acteurs[id] = asdict(update_data_acteur)
    save_to_file()
    return update_data_acteur

@app.delete("/acteurs/{id}")
def delete_acteur(id: int = Path(ge=1)) -> Acteur:
    if id in list_acteurs:
        acteur = Acteur(**list_acteurs[id])
        del list_acteurs[id]
        save_to_file()
        return acteur
    raise HTTPException(status_code=404, detail="cet acteur {id} n'existe pas ")
