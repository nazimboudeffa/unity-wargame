# Tactical Defense Commander

## 🎮 Concept du Jeu

**Tactical Defense Commander** est un jeu de défense tactique où vous gérez un budget de **$1,000,000** pour construire une armée et défendre votre base contre 10 vagues d'ennemis progressivement plus difficiles.

### Objectifs
- ✅ Survivre aux 10 vagues d'ennemis
- ✅ Gérer stratégiquement votre budget
- ✅ Choisir entre investir maintenant ou économiser pour plus tard
- ✅ Gérer vos munitions limitées avec précision

---

## 💰 Système Économique

### Budget Initial
- **$1,000,000** au départ

### Prix des Unités
| Unité | Prix | Caractéristiques |
|-------|------|------------------|
| **Tank Léger** | $50,000 | Rapide, faible armure |
| **Tank Moyen** | $120,000 | Équilibré |
| **Tank Lourd** | $250,000 | Lent, très résistant |
| **Lance-Missiles** | $180,000 | Longue portée, fragile |

### Prix des Munitions
| Type | Prix Unitaire | Caractéristiques |
|------|---------------|------------------|
| **Obus Standard** | $500 | Dégâts moyens |
| **Missile Guidé** | $2,500 | Haute précision |
| **Missile Anti-Tank** | $5,000 | Dégâts massifs |

### Récompenses
- **+$10,000** par ennemi détruit
- **Bonus de vague** : de $25,000 à $150,000 selon la difficulté

---

## 🎯 Mécaniques de Jeu

### Phase d'Achat (30-60 secondes)
1. Acheter des véhicules via l'interface UI
2. Acheter des munitions pour vos unités existantes
3. Placer stratégiquement vos unités
4. Cliquer sur "Démarrer la Vague" quand prêt

### Phase de Combat
1. Les ennemis apparaissent et avancent vers votre base
2. Contrôlez vos unités en **style RTS** :
   - **Clic gauche** : Sélectionner une unité alliée
   - **Clic droit** : Ordre d'attaque (sur ennemi) ou de déplacement (sur terrain)
   - **Espace** : Tir manuel (si une cible est sélectionnée)
   - **Échap** : Annuler la sélection
3. Les munitions se consomment à chaque tir
4. Défendez votre base jusqu'à l'élimination de tous les ennemis

### Victoire & Défaite
- **Victoire** : Survivre aux 10 vagues
- **Défaite** : Votre base est détruite

---

## 🛠️ Configuration dans Unity

### 1. Scripts Créés

#### Scripts Principaux
- **TacticalDefenseGameManager.cs** - Gère l'état global du jeu
- **GameEconomyManager.cs** - Gère le budget et les achats
- **WaveManager.cs** - Gère les vagues d'ennemis
- **ShopUI.cs** - Interface utilisateur du shop

#### Scripts de Gameplay
- **UnitData.cs** - Définitions des types d'unités et munitions
- **AmmunitionInventory.cs** - Inventaire de munitions par unité
- **UnitHealth.cs** - Système de santé et dégâts
- **EnemyAI.cs** - IA des ennemis
- **PlayerBase.cs** - Base du joueur à défendre
- **UnitInfoDisplay.cs** - Affichage santé/munitions au-dessus des unités

#### Scripts Mis à Jour
- **LaunchProjectile.cs** - Intégration du système de munitions
- **Projectile.cs** - Ajout des dégâts sur impact

### 2. Configuration de la Scène

#### A. GameManager Object
1. Créer un GameObject vide nommé "GameManager"
2. Ajouter les composants :
   - `TacticalDefenseGameManager`
   - `GameEconomyManager`
   - `WaveManager`

#### B. Configuration de GameEconomyManager

**Dans l'Inspector** :
1. **Starting Budget** : 1000000
2. **Available Units** : Créer 4 entrées
   ```
   [0] Light Tank
       - Type: LightTank
       - Display Name: "Tank Léger"
       - Purchase Cost: 50000
       - Prefab: [Votre prefab de tank léger]
       - Max Health: 100
       - Armor: 0
       - Max Ammo Capacity: 50
       - Starting Ammo: 10
   
   [1] Medium Tank
       - Type: MediumTank
       - Display Name: "Tank Moyen"
       - Purchase Cost: 120000
       - Prefab: [Votre prefab de tank moyen]
       - Max Health: 200
       - Armor: 20
       - Max Ammo Capacity: 40
       - Starting Ammo: 10
   
   [2] Heavy Tank
       - Type: HeavyTank
       - Display Name: "Tank Lourd"
       - Purchase Cost: 250000
       - Prefab: [Votre prefab de tank lourd]
       - Max Health: 400
       - Armor: 40
       - Max Ammo Capacity: 30
       - Starting Ammo: 10
   
   [3] Missile Launcher
       - Type: MissileLauncher
       - Display Name: "Lance-Missiles"
       - Purchase Cost: 180000
       - Prefab: [Votre prefab de lance-missiles]
       - Max Health: 80
       - Armor: 0
       - Max Ammo Capacity: 20
       - Starting Ammo: 5
   ```

3. **Available Ammo** : Créer 3 entrées
   ```
   [0] Standard Shell
       - Type: StandardShell
       - Display Name: "Obus Standard"
       - Cost: 500
       - Damage: 25
       - Projectile Prefab: [Votre prefab de missile]
   
   [1] Guided Missile
       - Type: GuidedMissile
       - Display Name: "Missile Guidé"
       - Cost: 2500
       - Damage: 50
       - Projectile Prefab: [Votre prefab de missile guidé]
   
   [2] Anti-Tank Missile
       - Type: AntiTankMissile
       - Display Name: "Missile Anti-Tank"
       - Cost: 5000
       - Damage: 100
       - Projectile Prefab: [Votre prefab de missile anti-tank]
   ```

4. **Rewards** :
   - Reward Per Enemy Killed: 10000
   - Wave Completion Bonus: [25000, 30000, 40000, 50000, 60000, 75000, 85000, 95000, 100000, 150000]

#### C. Configuration de WaveManager

1. **Spawn Points** : Créer plusieurs Transform vides placés aux points d'apparition des ennemis
2. **Delay Between Waves** : 15 (secondes)
3. **Total Waves To Win** : 10

4. **Waves** : Exemple de configuration
   ```
   [0] Vague 1
       - Wave Name: "Vague 1"
       - Enemy Count: 5
       - Spawn Interval: 2
       - Enemy Prefabs: [Prefab ennemi faible]
       - Enemy Health Multiplier: 1.0
   
   [1] Vague 2
       - Wave Name: "Vague 2"
       - Enemy Count: 8
       - Spawn Interval: 1.5
       - Enemy Prefabs: [Prefab ennemi faible, Prefab ennemi moyen]
       - Enemy Health Multiplier: 1.2
   
   [2] Vague 3
       - Wave Name: "Vague 3"
       - Enemy Count: 10
       - Spawn Interval: 1.5
       - Enemy Prefabs: [Prefab ennemi moyen]
       - Enemy Health Multiplier: 1.5
   
   ... (continuer jusqu'à la vague 10)
   
   [9] Vague 10 (Boss)
       - Wave Name: "Vague Finale"
       - Enemy Count: 20
       - Spawn Interval: 1
       - Enemy Prefabs: [Tous types d'ennemis]
       - Enemy Health Multiplier: 3.0
   ```

#### D. Configuration des Prefabs d'Unités Alliées

Pour chaque prefab de véhicule joueur :
1. Ajouter les composants :
   - `Selectable`
   - `UnitMovement`
   - `LaunchProjectile`
   - `AmmunitionInventory`
   - `UnitHealth`
   - `UnitInfoDisplay` (optionnel)

2. Configurer `LaunchProjectile` :
   - Launch Point: Transform du point de tir
   - Projectile Prefab: Prefab du missile
   - Target: (sera assigné dynamiquement)
   - Launch Velocity: 20
   - Fire Cooldown: 1.5

3. Les valeurs de `AmmunitionInventory` et `UnitHealth` seront définies par `UnitData`

#### E. Configuration des Prefabs Ennemis

**ÉTAPE 1 : Préparer le prefab et ajouter un Collider**

1. **Ouvrir le prefab** :
   - Dans le Project, trouvez votre prefab de tank/véhicule ennemi (ex: dans `Assets/Meshy_AI_Tank_low_poly...`)
   - Double-cliquez dessus pour l'ouvrir en mode Prefab

2. **Ajouter un Collider** (si pas déjà présent) :
   
   🔸 **Vérifier d'abord** : Regardez dans l'Inspector si le prefab a déjà un Collider
   
   🔸 **Si pas de Collider, en ajouter un** :
   
   **Méthode automatique (recommandée)** :
   - Sélectionnez l'objet racine du prefab dans la Hierarchy
   - Dans l'Inspector, cliquez sur **"Add Component"**
   - Recherchez **"Box Collider"** et cliquez dessus
   - Unity ajoute automatiquement un collider qui englobe le modèle
   - Ajustez la taille si nécessaire :
     - `Center` : Position du centre du collider (généralement Y=0.5 si le modèle est au sol)
     - `Size` : Dimensions X, Y, Z (visualisé par une boîte verte dans la Scene)
   
   **Types de Colliders disponibles** :
   
   | Type | Quand l'utiliser | Performance |
   |------|------------------|-------------|
   | **Box Collider** | Forme rectangulaire simple (tanks, bâtiments) | ⚡⚡⚡ Excellent |
   | **Capsule Collider** | Forme cylindrique (véhicules arrondis) | ⚡⚡⚡ Excellent |
   | **Sphere Collider** | Forme sphérique (projectiles, mines) | ⚡⚡⚡ Excellent |
   | **Mesh Collider** | Forme complexe précise (obstacles détaillés) | ⚡ Moyen (éviter sur ennemis mobiles) |
   
   **Exemple pour un tank** :
   ```
   Box Collider :
   - Center: X=0, Y=1, Z=0 (milieu du tank)
   - Size: X=3, Y=2, Z=5 (ajuster selon taille réelle)
   - Is Trigger: DÉCOCHÉ (collisions physiques normales)
   ```

3. **Visualiser le Collider** :
   - Dans la Scene view, le collider apparaît en **lignes vertes**
   - Utilisez l'outil **Rotate (E)** et **Scale (R)** pour vérifier que le collider englobe bien le modèle
   - Le collider peut être légèrement plus grand que le modèle visuel (c'est normal)

4. **Sauvegarder** :
   - Cliquez sur **"Save"** en haut de la fenêtre Prefab
   - Ou utilisez **Ctrl+S**

**ÉTAPE 2 : Ajouter les scripts de comportement**

Sélectionnez le prefab et dans l'Inspector, cliquez sur **"Add Component"** pour chaque script :

1. **Add Component** → Rechercher "Selectable" → Ajouter
   - ✅ Permet la sélection de l'ennemi (pour ciblage)

2. **Add Component** → Rechercher "Unit Movement" → Ajouter
   - Move Speed: `3` (plus lent que les unités alliées)
   - Stopping Distance: `0.2`
   - Movement Axes: `X=1, Y=0, Z=1` (mouvement horizontal)
   - Model Rotation Offset: `X=0, Y=90, Z=0` (ajuster selon orientation du modèle)

3. **Add Component** → Rechercher "Unit Health" → Ajouter
   - Max Health: `100` (santé de base, sera multipliée par le WaveManager)
   - Current Health: `100`
   - Armor: `0` (ou `10-20` pour des ennemis blindés)
   - ⚠️ **Is Player Unit**: DÉCOCHER (important!)
   - Destruction Effect Prefab: (optionnel) effet d'explosion

4. **Add Component** → Rechercher "Enemy AI" → Ajouter
   - Player Base: **Laisser vide** (trouvé automatiquement via Tag "PlayerBase")
   - Detection Range: `50`
   - ✅ **Move To Base**: COCHER
   - ✅ **Can Attack**: COCHER
   - Attack Range: `10` (distance pour commencer à tirer)
   - Attack Cooldown: `2` (secondes entre les tirs)
   - Attack Damage: `10` (dégâts si pas de lanceur de projectiles)

**ÉTAPE 3 : Configurer le Tag**

1. En haut de l'Inspector, trouvez le menu déroulant "Tag"
2. Sélectionnez **"Enemy"**
3. Si le tag n'existe pas :
   - Cliquez sur "Add Tag..."
   - Cliquez sur le "+" sous "Tags"
   - Entrez "Enemy"
   - Retournez au prefab et assignez le tag

**ÉTAPE 4 : (OPTIONNEL) Ajouter un système de tir**

Si vous voulez que l'ennemi tire des projectiles :

1. **Add Component** → "Launch Projectile" → Ajouter
   - Launch Point: Créer un Transform vide enfant à la position du canon
   - Projectile Prefab: Votre prefab de missile
   - Target: Laisser vide
   - Launch Velocity: `20`
   - Fire Cooldown: `2`

2. **Add Component** → "Ammunition Inventory" → Ajouter
   - Current Ammo: `999` (munitions illimitées pour les ennemis)
   - Max Ammo: `999`

**ÉTAPE 5 : (OPTIONNEL) Affichage de la barre de vie**

1. **Add Component** → "Unit Info Display" → Ajouter
   - Unit Health: Glisser le script UnitHealth du même objet
   - Show Ammo Count: Décocher (sauf si vous voulez voir les munitions ennemies)
   - Offset: `Y=3` (ajuster selon la taille du modèle)

**RÉSUMÉ : Composants finaux d'un prefab ennemi**

```
EnemyTank (Prefab)
├── Transform
├── Collider (BoxCollider/CapsuleCollider)
├── MeshFilter + MeshRenderer (votre modèle 3D)
├── Selectable ✅
├── UnitMovement ✅
├── UnitHealth ✅ (Is Player Unit = false)
├── EnemyAI ✅
├── LaunchProjectile (optionnel)
├── AmmunitionInventory (optionnel)
└── UnitInfoDisplay (optionnel)

Tag: "Enemy" ✅
```

**ÉTAPE 6 : Tester le prefab**

1. **Apply** les changements au prefab
2. Glissez le prefab dans une des **Waves** du WaveManager
3. Lancez le jeu et démarrez une vague pour vérifier :
   - ✅ L'ennemi spawn correctement
   - ✅ Il se déplace vers la base
   - ✅ Il attaque quand il arrive à portée
   - ✅ Il meurt quand sa santé atteint 0
   - ✅ Il donne la récompense ($10,000)

**TYPES D'ENNEMIS SUGGÉRÉS**

Créez plusieurs prefabs avec des stats différentes :

| Type | Health | Armor | Speed | Attack | Prix récompense suggéré |
|------|--------|-------|-------|--------|------------------------|
| **Scout** (Éclaireur) | 50 | 0 | 5 | 5 | $8,000 |
| **Standard** | 100 | 10 | 3 | 10 | $10,000 |
| **Heavy** (Lourd) | 300 | 30 | 1.5 | 15 | $15,000 |
| **Boss** | 800 | 40 | 2 | 25 | $50,000 |

#### F. Base du Joueur

**ÉTAPE 1 : Créer le Tag "PlayerBase" dans Unity**

⚠️ **Important** : Le tag doit exister avant de créer la base, sinon vous aurez une erreur !

1. **Créer le tag** :
   - En haut de Unity, cliquez sur **Tags and Layers** (ou n'importe où dans l'Inspector)
   - OU : Edit → Project Settings → Tags and Layers
   - Sous **"Tags"**, cliquez sur le **"+"**
   - Entrez "**PlayerBase**" (respectez la casse exacte)
   - Appuyez sur Entrée

2. **Vérifier que le tag existe** :
   - Le tag "PlayerBase" doit maintenant apparaître dans la liste des tags

**ÉTAPE 2 : Créer l'objet de la base**

1. **Créer un GameObject pour votre base** :
   - Dans la Hierarchy, **clic droit** → **Create Empty** (ou utilisez un cube/modèle 3D)
   - Nommez-le "**PlayerBase**"
   - Positionnez-le dans votre scène (ex: X=0, Y=0, Z=-20, derrière vos unités)

2. **Optionnel : Ajouter un visuel** :
   - Si vous avez utilisé Create Empty, ajoutez un visuel :
     - Clic droit sur PlayerBase → 3D Object → Cube
     - Ajustez la taille (Scale: X=5, Y=3, Z=5 par exemple)
   - OU glissez un modèle 3D de bâtiment sous PlayerBase

**ÉTAPE 3 : Ajouter les composants**

Sélectionnez **PlayerBase** dans la Hierarchy :

1. **Add Component** → Recherchez "**Player Base**" → Ajoutez-le
   - Max Health: **1000** (ou plus selon difficulté souhaitée)

2. **Add Component** → Recherchez "**Unit Health**" → Ajoutez-le
   - Max Health: **1000** (même valeur que PlayerBase)
   - Current Health: **1000**
   - Armor: **0** (ou ajoutez de l'armure pour plus de challenge)
   - ✅ **Is Player Unit**: COCHER (c'est une unité du joueur)
   - Destruction Effect Prefab: (optionnel) effet d'explosion

**ÉTAPE 4 : Assigner le Tag**

🔥 **Étape CRUCIALE** :

1. Avec **PlayerBase** toujours sélectionné
2. En haut de l'Inspector, trouvez le menu déroulant **"Tag"**
3. Cliquez dessus et sélectionnez **"PlayerBase"**
4. La base affiche maintenant "Tag: PlayerBase"

**ÉTAPE 5 : Tester**

1. Lancez le jeu
2. Ouvrez la **Console** (Ctrl+Shift+C)
3. Vérifiez qu'il n'y a **PLUS** l'erreur :
   - ❌ `UnityException: Tag: PlayerBase is not defined`
   - ✅ Si plus d'erreur → Le tag est correctement configuré !

4. Les ennemis devraient maintenant trouver la base automatiquement

**RÉSUMÉ : Configuration finale de la base**

```
PlayerBase (GameObject)
├── Transform (Position: ex. X=0, Y=0, Z=-20)
├── PlayerBase (Script) - Max Health: 1000
├── UnitHealth (Script) - Max Health: 1000, Is Player Unit: ✓
└── (Optionnel) Mesh/Model pour le visuel

Tag: "PlayerBase" ✅ OBLIGATOIRE
```

**Vérification rapide** :
- ✅ Le tag "PlayerBase" existe dans Project Settings → Tags
- ✅ L'objet PlayerBase a le tag "PlayerBase" assigné
- ✅ Les scripts PlayerBase et UnitHealth sont ajoutés
- ✅ Max Health est configuré (1000 recommandé)

#### G. Interface UI - Guide Complet

**🎨 CRÉATION DE L'UI DANS UNITY (Étape par étape)**

---

#### **PARTIE 1 : Créer le Canvas principal**

1. **Créer le Canvas** :
   - Dans la **Hierarchy** (fenêtre de gauche), **clic droit** dans le vide
   - **UI** → **Canvas**
   - Unity crée automatiquement :
     - **Canvas** (conteneur principal)
     - **EventSystem** (gère les clics/interactions)
   
2. **Configurer le Canvas** :
   - Sélectionnez **Canvas** dans la Hierarchy
   - Dans l'**Inspector** (fenêtre de droite) :
     - Render Mode: **Screen Space - Overlay** (par défaut)
     - Canvas Scaler → UI Scale Mode: **Scale With Screen Size**
     - Reference Resolution: **1920 x 1080**

3. **Ajouter le script ShopUI** :
   - Canvas toujours sélectionné
   - En bas de l'Inspector, cliquez **"Add Component"**
   - Tapez "**ShopUI**" → Cliquez dessus
   - Le script s'ajoute (tous les champs seront "None" pour l'instant)

---

#### **PARTIE 2 : Créer le texte du Budget (en haut à gauche)**

1. **Créer le texte** :
   - **Clic droit sur Canvas** dans la Hierarchy
   - **UI** → **Text - TextMeshPro**
   - ⚠️ Si c'est la première fois, Unity demande "Import TMP Essentials" → **Cliquez "Import" puis "Close"**
   - Un nouvel objet "Text (TMP)" apparaît sous Canvas

2. **Renommer** :
   - Sélectionnez le texte, appuyez sur **F2** ou clic droit → Rename
   - Nommez-le "**BudgetText**"

3. **Positionner** :
   - Sélectionnez **BudgetText**
   - Dans l'Inspector, trouvez **Rect Transform** :
     - Cliquez sur le carré avec l'ancre (en haut) → Maintenez **Alt** → Cliquez **top-left** (coin supérieur gauche)
     - Pos X: **200** (décalage depuis la gauche)
     - Pos Y: **-30** (décalage depuis le haut)
     - Width: **400**, Height: **60**

4. **Styliser** :
   - Toujours dans l'Inspector, section **TextMeshPro - Text (UI)** :
     - Text Input: "**Budget: $1,000,000**"
     - Font Size: **36**
     - Color: **Jaune or** (R:255, G:220, B:0) ou blanc
     - Alignment: Centré horizontalement, centré verticalement (les 3 boutons du milieu)

---

#### **PARTIE 3 : Créer le texte des infos de vague**

1. Répétez les étapes de la Partie 2, mais :
   - Nom: "**WaveInfoText**"
   - Position: **top-right** avec Alt
     - Pos X: **-200**, Pos Y: **-30**
   - Texte: "**Vague 1/10**"
   - Color: **Blanc**

---

#### **PARTIE 4 : Créer le bouton "Start Wave"**

1. **Créer le bouton** :
   - **Clic droit sur Canvas** → **UI** → **Button - TextMeshPro**
   - Un bouton avec du texte apparaît

2. **Renommer** :
   - Nommez le bouton "**StartWaveButton**"

3. **Positionner** :
   - Rect Transform → Ancre **bottom-center** (Alt + clic en bas au centre)
   - Pos X: **0**, Pos Y: **100**
   - Width: **300**, Height: **80**

4. **Changer le texte du bouton** :
   - Dans la Hierarchy, **dépliez StartWaveButton** (petit triangle à gauche)
   - Cliquez sur son enfant **"Text (TMP)"**
   - Dans l'Inspector :
     - Text Input: "**DÉMARRER LA VAGUE**"
     - Font Size: **28**
     - Color: **Blanc**
     - Font Style: **Bold** (B)

5. **Styliser le bouton** (optionnel) :
   - Sélectionnez **StartWaveButton** (parent)
   - Section Button (Script) → Colors :
     - Normal Color: Vert foncé
     - Highlighted Color: Vert clair
     - Pressed Color: Vert très clair

---

#### **PARTIE 5 : Créer les 4 boutons d'achat**

Vous allez créer 4 boutons similaires. Je vais détailler le premier, puis vous répéterez.

**1er bouton : Buy Light Tank**

1. **Créer** :
   - Clic droit Canvas → UI → Button - TextMeshPro
   - Nommez "**BuyLightTankButton**"

2. **Positionner** :
   - Ancre: **left-center** (Alt + clic au milieu à gauche)
   - Pos X: **220**, Pos Y: **150**
   - Width: **350**, Height: **60**

3. **Texte** :
   - Dépliez BuyLightTankButton → Cliquez sur "Text (TMP)"
   - Text Input: "**Tank Léger - $50,000**"
   - Font Size: **24**

**Répétez pour les 3 autres boutons** :

| Nom | Position Y | Texte | Prix |
|-----|-----------|-------|------|
| **BuyLightTankButton** | 150 | Tank Léger | $50,000 |
| **BuyMediumTankButton** | 80 | Tank Moyen | $120,000 |
| **BuyHeavyTankButton** | 10 | Tank Lourd | $250,000 |
| **BuyMissileLauncherButton** | -60 | Lance-Missiles | $180,000 |

Tous ont :
- Pos X: **220** (même alignement)
- Width: **350**, Height: **60**
- Ancre: **left-center**

---

#### **PARTIE 6 : Créer les textes de prix (optionnel)**

Si vous voulez des textes de prix séparés :

1. **Clic droit Canvas** → UI → Text - TextMeshPro
2. Nommez "**LightTankPriceText**"
3. Positionnez à droite du bouton correspondant
4. Texte: "**$50,000**"
5. Répétez pour les 3 autres

---

#### **PARTIE 7 : Créer le point de spawn des unités achetées**

1. **Créer un objet vide** :
   - Dans la Hierarchy (pas sous Canvas!), **clic droit** → **Create Empty**
   - Nommez "**UnitSpawnPoint**"

2. **Positionner dans la scène** :
   - Sélectionnez **UnitSpawnPoint**
   - Dans la **Scene view** (vue 3D), positionnez-le devant votre base
   - Exemple de position : X=0, Y=0, Z=10
   - C'est là que les unités achetées apparaîtront

---

#### **PARTIE 8 : Connecter l'UI au script ShopUI**

**C'est l'étape CRUCIALE !**

1. **Sélectionnez Canvas** dans la Hierarchy

2. **Dans l'Inspector**, trouvez le composant **ShopUI**

3. **Glisser-déposer chaque élément** :
   
   Depuis la **Hierarchy**, **glissez** chaque objet dans le champ correspondant du ShopUI :
   
   ```
   ShopUI (Script)
   ├── Budget Text: [Glissez BudgetText ici]
   ├── Wave Info Text: [Glissez WaveInfoText ici]
   ├── Start Wave Button: [Glissez StartWaveButton ici]
   ├── Shop Panel: [Glissez Canvas ici, ou laissez None pour une version simple]
   │
   ├── Buy Light Tank Button: [Glissez BuyLightTankButton ici]
   ├── Buy Medium Tank Button: [Glissez BuyMediumTankButton ici]
   ├── Buy Heavy Tank Button: [Glissez BuyHeavyTankButton ici]
   ├── Buy Missile Launcher Button: [Glissez BuyMissileLauncherButton ici]
   │
   ├── Light Tank Price Text: [Glissez LightTankPriceText ici, ou None]
   ├── Medium Tank Price Text: [...]
   ├── Heavy Tank Price Text: [...]
   ├── Missile Launcher Price Text: [...]
   │
   └── Unit Spawn Point: [Glissez UnitSpawnPoint ici]
   ```

4. **Vérifier** :
   - Aucun champ essentiel ne doit rester "None"
   - Si un champ reste "None", l'UI correspondante ne fonctionnera pas

---

#### **PARTIE 9 : Panneaux Victory/Defeat (optionnels mais recommandés)**

**Panel Victory** :

1. Clic droit Canvas → UI → Panel
2. Nommez "**VictoryPanel**"
3. Rect Transform → Couvrir tout l'écran (Stretch/Stretch en ancrant)
4. Image → Color: Vert transparent (A=180)
5. Ajoutez un texte enfant : "VICTOIRE!"
6. Ajoutez 2 boutons enfants : "Restart", "Quit"
7. **Désactiver le panel au départ** : Décochez la case à côté du nom dans l'Inspector

**Panel Defeat** :
- Même chose, mais en rouge et texte "DÉFAITE!"

Dans ShopUI, glissez :
- Victory Panel → VictoryPanel
- Defeat Panel → DefeatPanel

---

#### **PARTIE 10 : Test final**

1. **Sauvegardez la scène** : Ctrl+S

2. **Lancez le jeu** : Bouton Play ▶️ en haut

3. **Vérifications** :
   - ✅ Vous voyez "Budget: $1,000,000" en haut à gauche
   - ✅ Vous voyez les 4 boutons d'achat à gauche
   - ✅ Vous voyez le bouton "DÉMARRER LA VAGUE" en bas
   - ✅ Survoler les boutons change leur couleur (highlight)

4. **Test d'achat** :
   - Cliquez sur "Tank Léger - $50,000"
   - Ouvrez la Console (Ctrl+Shift+C)
   - Vous devriez voir : `[Economy] Tank Léger acheté...`
   - Si erreur "Prefab manquant" → Configurez GameEconomyManager (Partie B)

---

### **🎨 Hiérarchie finale attendue**

```
Hierarchy
├── Canvas
│   ├── BudgetText
│   ├── WaveInfoText
│   ├── StartWaveButton
│   │   └── Text (TMP)
│   ├── BuyLightTankButton
│   │   └── Text (TMP)
│   ├── BuyMediumTankButton
│   │   └── Text (TMP)
│   ├── BuyHeavyTankButton
│   │   └── Text (TMP)
│   ├── BuyMissileLauncherButton
│   │   └── Text (TMP)
│   ├── VictoryPanel (désactivé)
│   └── DefeatPanel (désactivé)
├── EventSystem
├── UnitSpawnPoint
└── ... (vos autres objets de scène)
```

---

### **⚠️ Problèmes courants lors de la création de l'UI**

| Problème | Solution |
|----------|----------|
| Les boutons ne réagissent pas aux clics | Vérifiez qu'EventSystem existe dans la Hierarchy |
| L'UI est invisible | Vérifiez Canvas → Render Mode = Screen Space - Overlay |
| Les textes sont flous | Canvas Scaler → Scale With Screen Size |
| Les boutons sont mal positionnés | Utilisez les ancres (Alt+clic) pour fixer aux coins |
| Erreur "ShopUI not found" | Le script est dans Assets/Scripts/, vérifiez qu'il compile |
| Les clics passent à travers l'UI | Canvas → Graphic Raycaster doit être présent |

---

**✅ Une fois l'UI créée et connectée, vous pourrez :**
- Voir votre budget en temps réel
- Acheter des unités avec les boutons
- Démarrer les vagues
- Voir les écrans de victoire/défaite

Passez ensuite à la configuration du GameEconomyManager (Partie B) pour que les achats fonctionnent !

---

## 🎲 Stratégies de Jeu

### Stratégies Recommandées

1. **Démarrage Prudent**
   - Acheter 2-3 tanks moyens ($240k-$360k)
   - Garder $640k+ pour les vagues difficiles
   - Économiser sur les munitions au début

2. **Investissement Agressif**
   - Acheter 1 tank lourd + 2 lance-missiles ($610k)
   - Maximiser les dégâts tôt
   - Risqué si on perd des unités

3. **Économie Pure**
   - 1 tank léger + munitions minimales ($60k)
   - Farmer les premières vagues
   - Gros investissement vagues 5-7

4. **Équilibre**
   - Mix de tanks légers et moyens ($400k)
   - Réinvestir les récompenses progressivement
   - S'adapter selon les pertes

### Gestion des Munitions
- **Ne pas gaspiller** : Chaque tir compte!
- **Cibler intelligemment** : Prioriser les ennemis les plus dangereux
- **Recharger stratégiquement** : Profiter des pauses entre vagues
- **Calculer** : $500/munition × 50 tirs = $25,000 de coût opérationnel

---

## 🐛 Debug & Tests

### ⚠️ DÉPANNAGE : Les unités achetées ne spawent pas

**PROBLÈME : Je clique sur "Buy Tank" mais rien n'apparaît**

#### 🚀 Vérifications pour le spawn des unités joueur

**1. Vérifier que le UnitSpawnPoint existe**

- Dans la **Hierarchy**, cherchez un objet **"UnitSpawnPoint"**
- ❌ **Si absent** :
  ```
  SOLUTION :
  1. Clic droit Hierarchy → Create Empty
  2. Nommez-le "UnitSpawnPoint"
  3. Positionnez-le dans la scène (ex: X=0, Y=0, Z=10 devant votre base)
  4. Glissez-le dans le champ "Unit Spawn Point" du ShopUI sur le Canvas
  ```

**2. Vérifier que ShopUI a le UnitSpawnPoint assigné**

- Sélectionnez **Canvas** dans la Hierarchy
- Dans l'Inspector, trouvez le composant **ShopUI**
- Vérifiez le champ **"Unit Spawn Point"**
- ❌ **Si "None"** :
  ```
  SOLUTION :
  Glissez l'objet UnitSpawnPoint depuis la Hierarchy vers ce champ
  ```

**3. Vérifier que GameEconomyManager a les prefabs des unités**

- Sélectionnez **GameManager** dans la Hierarchy
- Dans l'Inspector, trouvez **GameEconomyManager**
- Section **"Available Units"**
- ❌ **Si Size: 0** :
  ```
  SOLUTION :
  1. Changez "Size" à 4
  2. Configurez chaque unité avec son prefab (voir Section B)
  ```
- ✅ **Si Size: 4**, vérifiez CHAQUE entrée :
  - Champ **"Prefab"** ne doit PAS être "None"
  - ❌ **Si "None"** : Glissez votre prefab d'unité depuis le Project

**4. Vérifier que les prefabs d'unités existent**

- Trouvez vos modèles de tanks dans le **Project** (ex: dans Assets/Meshy_AI_Tank...)
- Vérifiez qu'ils sont bien des **prefabs** (icône bleue)
- ❌ **Si ce ne sont pas des prefabs** :
  ```
  SOLUTION :
  1. Glissez le modèle 3D dans la scène
  2. Ajoutez les composants nécessaires (voir Section D)
  3. Glissez l'objet depuis la Hierarchy vers le dossier Project
  4. Unity crée un prefab (icône bleue)
  5. Supprimez l'instance de la scène (Hierarchy)
  ```

**5. Vérifier que les prefabs ont les scripts nécessaires**

Sélectionnez un prefab d'unité joueur et vérifiez qu'il a :
- ✅ `Selectable`
- ✅ `UnitMovement`
- ✅ `LaunchProjectile` (avec Projectile Prefab assigné)
- ✅ `AmmunitionInventory`
- ✅ `UnitHealth`

❌ **Si manquants** : Voir Section D - Configuration des Prefabs d'Unités Alliées

**6. TEST : Vérifier la Console**

- Lancez le jeu et cliquez sur "Buy Tank Léger"
- Ouvrez la **Console** (Ctrl+Shift+C)
- Cherchez les messages :
  - ✅ `[Economy] Tank Léger acheté pour $50000...` + `[ShopUI] Tank Léger acheté et placé!`
    → **Ça marche !** L'unité devrait apparaître au UnitSpawnPoint
  
  - ❌ `[Economy] Prefab manquant pour Tank Léger`
    → Le prefab n'est pas assigné dans GameEconomyManager
  
  - ❌ `[Economy] Fonds insuffisants...`
    → Appuyez sur **M** pour ajouter $100k (code debug)
  
  - ❌ `[Economy] UnitData est null!`
    → GameEconomyManager n'est pas configuré (Available Units vide)
  
  - ❌ Rien du tout
    → Les boutons ne sont pas connectés au ShopUI

**7. TEST VISUEL : Chercher l'unité dans la scène**

Si le message de succès apparaît mais vous ne voyez pas l'unité :

1. **Vérifier la position du UnitSpawnPoint** :
   - Sélectionnez UnitSpawnPoint dans la Hierarchy
   - Regardez sa position (Transform) : X, Y, Z
   - Est-elle visible dans votre caméra ?
   - Déplacez-la si nécessaire (ex: X=0, Y=0, Z=10)

2. **Chercher dans la Hierarchy** :
   - Après l'achat, un nouvel objet devrait apparaître
   - Nom : "Tank(Clone)" ou similaire
   - ❌ Si absent : Le spawn a échoué
   - ✅ Si présent : L'unité existe mais est peut-être hors caméra

3. **Vérifier l'échelle du prefab** :
   - Le modèle est peut-être trop petit ou trop grand
   - Sélectionnez le prefab → Transform → Scale
   - Ajustez si nécessaire (ex: 1, 1, 1 ou 0.1, 0.1, 0.1)

**8. 🔍 DIAGNOSTIC AVEC LES NOUVEAUX LOGS DE DEBUG**

✅ **Les scripts ont maintenant des logs DEBUG très détaillés !**

**COMMENT DIAGNOSTIQUER** :

1. **Lancez le jeu** (bouton Play ▶️)
2. **Ouvrez la Console** (Ctrl+Shift+C)
3. **Cliquez sur un bouton d'achat** (ex: "Buy Tank Léger")
4. **Lisez les messages dans cet ordre** :

```
SÉQUENCE ATTENDUE SI TOUT FONCTIONNE :
✅ [ShopUI DEBUG] === DÉBUT ACHAT LightTank ===
✅ [ShopUI DEBUG] GameEconomyManager trouvé
✅ [ShopUI DEBUG] UnitData trouvé: Tank Léger, Prix: 50000
✅ [ShopUI DEBUG] UnitSpawnPoint OK à position: (0.0, 0.0, 10.0)
✅ [Economy DEBUG] === DÉBUT TryPurchaseUnit ===
✅ [Economy DEBUG] UnitData OK: Tank Léger
✅ [Economy DEBUG] Budget actuel: $1000000, Coût: $50000
✅ [Economy DEBUG] Prefab OK: Tank_Prefab
✅ [Economy DEBUG] Budget déduit. Nouveau budget: $950000
✅ [Economy DEBUG] Instantiation du prefab...
✅ [Economy DEBUG] Prefab instantié: Tank_Prefab(Clone)
✅ [Economy DEBUG] Munitions initialisées: 10/50
✅ [Economy DEBUG] Santé initialisée: 100 HP, 0 armor
✅ [Economy DEBUG] ✅ Tank Léger acheté pour $50000. Budget restant: $950000
✅ [ShopUI DEBUG] TryPurchaseUnit a retourné TRUE
✅ [ShopUI DEBUG] spawnedUnit créé: Tank_Prefab(Clone)
✅ [ShopUI DEBUG] ✅ Tank Léger SPAWNÉ à (0.0, 0.0, 10.0)!
```

**SI VOUS VOYEZ UNE ERREUR, VOICI LES SOLUTIONS** :

---

**❌ ERREUR 1 : `GameEconomyManager.Instance est NULL!`**
```
PROBLÈME : Le GameManager n'existe pas dans la scène
SOLUTION :
1. Hierarchy → Clic droit → Create Empty
2. Nommez "GameManager"
3. Add Component → GameEconomyManager
4. Add Component → WaveManager
5. Add Component → TacticalDefenseGameManager
```

---

**❌ ERREUR 2 : `Impossible de trouver les données pour LightTank`**
```
PROBLÈME : GameEconomyManager.availableUnits est vide
SOLUTION :
1. Sélectionnez GameManager
2. GameEconomyManager → Available Units → Size: 4
3. Configurez chaque entrée (voir Section B du guide)
```

---

**❌ ERREUR 3 : `⚠️ UNITSPAWNPOINT EST NULL! Assignez-le dans l'Inspector!`**
```
PROBLÈME : Le champ unitSpawnPoint du ShopUI n'est pas assigné
SOLUTION :
1. Créez un GameObject vide : Hierarchy → Create Empty
2. Nommez-le "UnitSpawnPoint"
3. Positionnez-le (ex: X=0, Y=0, Z=10)
4. Sélectionnez Canvas
5. Dans ShopUI, glissez UnitSpawnPoint dans "Unit Spawn Point"
```

---

**❌ ERREUR 4 : `⚠️ PREFAB MANQUANT pour Tank Léger! Assignez-le dans GameEconomyManager → Available Units!`**
```
PROBLÈME : Le prefab d'unité n'est pas assigné
SOLUTION :
1. Sélectionnez GameManager
2. GameEconomyManager → Available Units → [0] Light Tank
3. Dans le champ "Prefab", glissez votre prefab de tank depuis le Project
4. Le prefab doit être une icône BLEUE dans le Project
5. Si ce n'est pas un prefab, créez-en un :
   - Glissez le modèle 3D dans la scène
   - Ajoutez les composants nécessaires
   - Glissez depuis Hierarchy vers un dossier du Project
```

---

**❌ ERREUR 5 : `⚠️ Fonds insuffisants pour Tank Léger. Besoin: $50000, Disponible: $0`**
```
PROBLÈME : Le budget n'a pas été initialisé
SOLUTION :
1. Vérifiez que le jeu a bien démarré (Play activé)
2. GameEconomyManager → Starting Budget: 1000000
3. OU appuyez sur M pour ajouter $100k (debug)
```

---

**❌ ERREUR 6 : `TryPurchaseUnit a retourné FALSE`**
```
PROBLÈME : L'achat a échoué (budget ou prefab)
SOLUTION : Regardez les messages JUSTE AU-DESSUS dans la Console
Ils vous diront exactement pourquoi (budget insuffisant ou prefab manquant)
```

---

**✅ SI TOUS LES MESSAGES S'AFFICHENT MAIS VOUS NE VOYEZ PAS L'UNITÉ :**

L'unité a spawné mais elle est hors caméra !

1. **Trouvez l'unité dans la Hierarchy** :
   - Cherchez un objet nommé "Tank_Prefab(Clone)" ou similaire
   - Sélectionnez-le
   - Appuyez sur **F** pour centrer la caméra dessus (dans Scene view)

2. **Vérifiez la position du UnitSpawnPoint** :
   - Elle doit être VISIBLE pour votre caméra de jeu
   - Exemple : X=0, Y=0, Z=10 (devant la base)
   - Ajustez si nécessaire

3. **Vérifiez l'échelle du prefab** :
   - Le modèle est peut-être trop petit
   - Sélectionnez le prefab → Scale : X=1, Y=1, Z=1
   - Ou X=10, Y=10, Z=10 s'il est très petit

---

**CHECKLIST RAPIDE** :
- [ ] Console affiche `[ShopUI DEBUG] GameEconomyManager trouvé`
- [ ] Console affiche `[ShopUI DEBUG] UnitSpawnPoint OK à position: ...`
- [ ] Console affiche `[Economy DEBUG] Prefab OK: ...`
- [ ] Console affiche `[Economy DEBUG] ✅ ... acheté pour $...`
- [ ] Console affiche `[ShopUI DEBUG] ✅ ... SPAWNÉ à ...`
- [ ] Budget diminue après l'achat (visible en haut à gauche)
- [ ] UnitSpawnPoint positionné dans le champ de vision de la caméra

---

### ⚠️ DÉPANNAGE : Les ennemis ne spawent pas

**PROBLÈME : "Buy Phase" s'affiche mais rien ne se passe**

Si vous voyez "Buy Phase" mais ne pouvez rien faire :

#### 🛒 Vérifications pour l'interface d'achat

**1. Vérifier que l'UI existe et est visible**

- Dans la **Hierarchy**, cherchez un objet **Canvas**
- ❌ **Si absent** :
  ```
  SOLUTION :
  1. Clic droit dans Hierarchy → UI → Canvas
  2. Unity crée automatiquement : Canvas + EventSystem
  3. Continuez avec l'étape 2
  ```
- Vérifiez que l'objet **EventSystem** existe aussi (créé automatiquement avec Canvas)

**2. Vérifier le script ShopUI**

- Sélectionnez votre **Canvas**
- Dans l'Inspector, cherchez le composant **ShopUI**
- ❌ **Si absent** : Add Component → Recherchez "ShopUI" → Ajoutez-le

**3. Vérifier les références UI dans ShopUI**

Sélectionnez le Canvas avec ShopUI, dans l'Inspector vérifiez :

```
ShopUI Component:
├── Budget Text: [Doit pointer vers votre TextMeshProUGUI du budget]
├── Wave Info Text: [Doit pointer vers votre TextMeshProUGUI des infos de vague]
├── Start Wave Button: [Doit pointer vers votre Button "Start Wave"]
├── Buy Light Tank Button: [Doit pointer vers votre Button "Buy Light Tank"]
├── Buy Medium Tank Button: [...]
├── Buy Heavy Tank Button: [...]
├── Buy Missile Launcher Button: [...]
└── Unit Spawn Point: [Transform où les unités achetées apparaissent]
```

❌ **Si tous ces champs sont "None"**, vous devez créer l'UI :

**SOLUTION RAPIDE : Créer une UI minimale de test**

1. **Créer le Canvas** (si pas déjà fait) :
   - Clic droit Hierarchy → UI → Canvas

2. **Créer le texte du budget** :
   - Clic droit sur Canvas → UI → Text - TextMeshPro
   - Si demandé, cliquez "Import TMP Essentials"
   - Nommez-le "BudgetText"
   - Positionnez en haut à gauche
   - Texte par défaut : "Budget: $1000000"

3. **Créer le bouton "Start Wave"** :
   - Clic droit sur Canvas → UI → Button - TextMeshPro
   - Nommez-le "StartWaveButton"
   - Changez le texte du bouton (enfant Text) à "START WAVE"
   - Positionnez au centre-bas

4. **Créer les boutons d'achat** :
   - Répétez pour chaque unité :
     - Clic droit Canvas → UI → Button - TextMeshPro
     - Nommez "BuyLightTankButton", "BuyMediumTankButton", etc.
     - Changez le texte : "Buy Light Tank ($50k)", etc.
     - Positionnez-les verticalement sur le côté

5. **Créer un Transform pour le spawn** :
   - Clic droit Hierarchy → Create Empty
   - Nommez "UnitSpawnPoint"
   - Positionnez devant votre base (où les unités achetées apparaîtront)

6. **Connecter tout au ShopUI** :
   - Sélectionnez le Canvas
   - Glissez chaque élément UI dans le champ correspondant du ShopUI

**4. Vérifier que GameEconomyManager a des UnitData**

- Sélectionnez **GameManager** dans la Hierarchy
- Trouvez le composant **GameEconomyManager**
- Vérifiez **Available Units** :
  - ❌ **Si "Size: 0"** : Vous devez configurer les unités !
  
  ```
  SOLUTION :
  1. Changez "Size" à 4
  2. Pour chaque entrée [0] à [3] :
     - Type: LightTank, MediumTank, HeavyTank, MissileLauncher
     - Display Name: "Tank Léger", "Tank Moyen", etc.
     - Purchase Cost: 50000, 120000, 250000, 180000
     - Prefab: Glissez votre prefab d'unité joueur ⚠️ IMPORTANT
     - Max Health: 100, 200, 400, 80
     - Armor: 0, 20, 40, 0
     - Max Ammo Capacity: 50, 40, 30, 20
     - Starting Ammo: 10, 10, 10, 5
  ```

**5. TEST SIMPLE : Vérifier la Console**

- Lancez le jeu
- Ouvrez la **Console** (Ctrl+Shift+C)
- Cliquez sur un bouton d'achat
- Cherchez ces messages :
  - ✅ `[Economy] Tank Léger acheté pour $50000...` → Ça marche !
  - ❌ `[Economy] Prefab manquant...` → Assignez les prefabs dans GameEconomyManager
  - ❌ `[Economy] Fonds insuffisants...` → Budget trop bas (utilisez M pour +$100k)
  - ❌ Rien du tout → Les boutons ne sont pas connectés au ShopUI

**6. TEST : Démarrer une vague manuellement**

Si l'UI ne marche toujours pas, testez avec le clavier :
- Appuyez sur **N** (si vous avez ajouté le code debug)
- OU dans la Console, tapez (quand le jeu est en pause) :
  ```
  TacticalDefenseGameManager.Instance.StartCombatPhase();
  ```

---

### ⚠️ DÉPANNAGE : Les ennemis ne spawent pas

**VÉRIFICATIONS ESSENTIELLES** :

#### 1️⃣ Vérifier que le GameManager existe dans la scène

- Ouvrez votre scène de jeu
- Dans la **Hierarchy**, cherchez un objet nommé "**GameManager**"
- ❌ **Si absent** : 
  - Clic droit dans Hierarchy → Create Empty
  - Nommez-le "GameManager"
  - Ajoutez les 3 composants : `TacticalDefenseGameManager`, `GameEconomyManager`, `WaveManager`

#### 2️⃣ Vérifier les Spawn Points

- Sélectionnez le **GameManager** dans la Hierarchy
- Dans l'Inspector, trouvez le composant **WaveManager**
- Vérifiez le champ **Spawn Points** :
  - ❌ **Si "Size: 0" ou vide** :
    ```
    SOLUTION :
    1. Créez des GameObjects vides dans la scène (Clic droit → Create Empty)
    2. Nommez-les "SpawnPoint1", "SpawnPoint2", etc.
    3. Placez-les aux endroits d'apparition des ennemis (loin de la base)
    4. Glissez-les dans le tableau "Spawn Points" du WaveManager
    ```
  - ✅ **Doit avoir** : Au moins 1 Transform assigné

#### 3️⃣ Vérifier les Waves

- Toujours dans **WaveManager**, trouvez la section **Waves**
- ❌ **Si "Size: 0"** :
  ```
  SOLUTION :
  1. Changez "Size" à 10 (pour 10 vagues)
  2. Configurez chaque vague avec :
     - Wave Name: "Vague 1", "Vague 2", etc.
     - Enemy Count: 5, 8, 10, etc. (nombre d'ennemis)
     - Spawn Interval: 2 (secondes entre chaque spawn)
     - Enemy Prefabs: Glissez au moins 1 prefab d'ennemi ici ⚠️
     - Enemy Health Multiplier: 1.0, 1.2, 1.5, etc.
  ```

#### 4️⃣ Vérifier que les prefabs sont assignés

- Dans chaque **Wave**, vérifiez **Enemy Prefabs**
- ❌ **Si "None" ou vide** :
  ```
  SOLUTION :
  1. Dans le Project, trouvez votre prefab d'ennemi configuré
  2. Glissez-le dans le champ "Enemy Prefabs" de la vague
  3. Vous pouvez ajouter plusieurs prefabs (différents types)
  ```

#### 5️⃣ Vérifier que la vague démarre

- Lancez le jeu (Play)
- Ouvrez la **Console** (Window → General → Console) ou (Ctrl+Shift+C)
- Cherchez ces messages :
  - ✅ `[WaveManager] === Vague 1 commence! X ennemis ===`
  - ❌ Si absent : La vague n'a pas démarré

- **Comment démarrer une vague** :
  ```
  OPTION A : Via l'UI
  - Cliquez sur le bouton "Start Wave" dans votre interface

  OPTION B : Automatique (pour tester)
  - Sélectionnez GameManager → TacticalDefenseGameManager
  - Cochez "Auto Start Waves"

  OPTION C : Manuellement via code de debug
  - Appuyez sur la touche "N" (si vous ajoutez le code debug ci-dessous)
  ```

#### 6️⃣ Vérifier les erreurs dans la Console

- Si des **erreurs rouges** apparaissent, lisez-les :
  - `NullReferenceException` → Un composant ou prefab n'est pas assigné
  - `Aucun spawn point défini` → Assignez des spawn points (étape 2)
  - `Aucun prefab d'ennemi` → Assignez des prefabs (étape 4)
  - `Tag: PlayerBase is not defined` → **Créez le tag "PlayerBase"** (voir section F - Base du Joueur, ÉTAPE 1)
  - `Tag: Enemy is not defined` → **Créez le tag "Enemy"** (voir section E - Prefabs Ennemis, ÉTAPE 3)

**SOLUTION Tag manquant** :
```
1. Edit → Project Settings → Tags and Layers
2. Sous "Tags", cliquez sur "+"
3. Entrez "PlayerBase" ou "Enemy" selon le tag manquant
4. Retournez à votre objet et assignez le tag dans l'Inspector (menu déroulant Tag en haut)
```

#### 7️⃣ TEST RAPIDE : Spawn forcé

Ajoutez ce code temporaire pour tester :

Dans **WaveManager.cs**, ajoutez dans `Update()` :

```csharp
void Update()
{
    // TEST: Appuyez sur T pour spawner un ennemi de test
    if (Input.GetKeyDown(KeyCode.T) && waves.Count > 0 && waves[0].enemyPrefabs.Length > 0)
    {
        GameObject testEnemy = Instantiate(waves[0].enemyPrefabs[0], 
            spawnPoints[0].position, 
            spawnPoints[0].rotation);
        Debug.Log("[TEST] Ennemi spawné manuellement : " + testEnemy.name);
    }
}
```

- Lancez le jeu et appuyez sur **T**
- ✅ **Si un ennemi apparaît** → Le problème vient du système de démarrage des vagues
- ❌ **Si rien ne se passe** → Le problème vient des prefabs ou spawn points

---

### Commandes de Debug (à ajouter si nécessaire)

Ajoutez ces lignes dans **TacticalDefenseGameManager.cs** `Update()` pour faciliter les tests :

```csharp
void Update()
{
    // ... code existant ...
    
    // DEBUG: Ajouter $100k
    if (Input.GetKeyDown(KeyCode.M))
    {
        GameEconomyManager.Instance.AddFunds(100000);
        Debug.Log("[DEBUG] +$100,000 ajoutés");
    }

    // DEBUG: Lancer vague immédiatement
    if (Input.GetKeyDown(KeyCode.N))
    {
        StartCombatPhase();
        Debug.Log("[DEBUG] Vague démarrée manuellement");
    }
    
    // DEBUG: Afficher l'état
    if (Input.GetKeyDown(KeyCode.I))
    {
        Debug.Log($"[DEBUG] État: {_currentState}, Vague: {WaveManager.Instance.CurrentWaveIndex + 1}, " +
                  $"Ennemis restants: {WaveManager.Instance.EnemiesRemaining}, " +
                  $"Budget: ${GameEconomyManager.Instance.CurrentBudget}");
    }
}
```

**Touches de debug** :
- **M** : +$100,000
- **N** : Démarrer la vague suivante
- **I** : Afficher les infos (état, vague, ennemis, budget)
- **T** : Spawner un ennemi de test (si ajouté ci-dessus)

---

### Tests Recommandés
1. ✅ Acheter chaque type d'unité
2. ✅ Vérifier la consommation de munitions
3. ✅ Tester la mort d'une unité (perte permanente)
4. ✅ Tester la victoire (10 vagues)
5. ✅ Tester la défaite (base détruite)
6. ✅ Vérifier les récompenses
7. ✅ Spawner des ennemis (appuyez sur N puis vérifiez)

---

## 📝 Notes de Développement

### Prochaines Améliorations Possibles
- [ ] Système de placement manuel des unités (drag & drop)
- [ ] Types d'ennemis variés (rapides, blindés, aériens)
- [ ] Power-ups et capacités spéciales
- [ ] Mode multijoueur coop
- [ ] Système de sauvegarde/chargement
- [ ] Statistiques de fin de partie
- [ ] Leaderboard
- [ ] Sons et musique
- [ ] Effets visuels améliorés

### Fichiers Créés
```
Assets/Scripts/
├── TacticalDefenseGameManager.cs
├── GameEconomyManager.cs
├── WaveManager.cs
├── ShopUI.cs
├── UnitData.cs
├── AmmunitionInventory.cs
├── UnitHealth.cs
├── EnemyAI.cs
├── PlayerBase.cs
├── UnitInfoDisplay.cs
├── LaunchProjectile.cs (modifié)
└── Projectile.cs (modifié)
```

---

## 🎮 Contrôles

| Action | Contrôle |
|--------|----------|
| Sélectionner unité alliée | Clic gauche |
| Ordre de mouvement | Clic droit (sur terrain) |
| Ordre d'attaque | Clic droit (sur ennemi) |
| Tir manuel | Espace |
| Annuler sélection | Échap |

---

**Bon jeu et bonne défense!** 🎯🚀
