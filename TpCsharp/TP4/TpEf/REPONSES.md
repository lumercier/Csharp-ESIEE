# Réponses TP4 - EF Core

## Étape 1

**Pourquoi les colonnes Nom ou Matiere sont-elles NOT NULL alors que vous ne l'avez écrit nulle part ?**   
`Nom` et `Matiere` sont déclarées comme `string` et pas `string?` donc elles ne peuvent pas être nulles. EF Core le voit et ajoute tout seul `NOT NULL` dans la base de données.

**Qu'est-ce qui a permis à EF Core de deviner que EtudiantId est une clé étrangère ?**   
`EtudiantId` commence par le nom de la classe `Etudiant` suivi de `Id` (et qu'elle a le même type `int`) il comprend que c'est le lien vers la table `Etudiants` (donc une clé étrangère).

## Étape 3

**Pourquoi Q12 donne-t-elle 0 sans Include ? Comment s'appelle le problème qu'on aurait si l'on chargeait les notes de chaque étudiant dans la boucle ?**
EF Core ne charge que la table demandée et laisse la liste `Notes` vide pour ne pas ralentir la requête. Charger les notes de chaque étudiant une par une ferait beaucoup trop de requetes avec la base.

**Q13 : que se passerait-il si l'on gardait Hamilton, qui n'a aucune note ?**   
Hamilton n'a aucune note donc sa moyenne vaut 0, ce qui ferait une division par zéro et ferait crasher le programme.

## Étape 4

**Pourquoi n'a-t-on appelé aucune méthode Update à la question 14 ?**
Parce que l'objet `alan` a été récupéré via `db`, et EF Core le surveille en mémoire.

**Après ExecuteUpdate, les objets Note déjà chargés dans le contexte ont-ils leur nouveau coefficient ? Pourquoi ?**
Non car `ExecuteUpdate` modifie les données directement dans la base SQL. Les objets sot déjà chargés et donc gardent donc leur ancienne valeur.