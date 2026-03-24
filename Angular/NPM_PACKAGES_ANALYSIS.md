# Analyse des paquets npm — Angular

> Généré le 2026-03-24 à partir des commandes `npm install` et `npm audit` dans le dossier `Angular/`.

---

## Table des matières

1. [Avertissements d'installation (`npm install`)](#1-avertissements-dinstallation-npm-install)
2. [Vulnérabilités de sécurité (`npm audit`)](#2-vulnérabilités-de-sécurité-npm-audit)
3. [Solutions proposées](#3-solutions-proposées)
4. [Rôles des paquets npm par fonctionnalité](#4-rôles-des-paquets-npm-par-fonctionnalité)

---

## 1. Avertissements d'installation (`npm install`)

> **État après migration** : suite à la suppression de `prettier-eslint` et `eslint-plugin-prettier`, les 6 premiers avertissements ci-dessous ont été résolus. Seul `lodash.isequal` (via `quill`) demeure, en attente d'une version corrective de Quill.

### 1.1 Tableau des avertissements (état initial → état actuel)

| Paquet déprécié | Version installée | Motif | Chaîne de dépendance (depuis `package.json`) | Statut |
|---|---|---|---|---|
| `eslint` | 8.57.1 | Version dépréciée/non supportée | `prettier-eslint@~16.4.2` → `eslint@8.57.1` | ✅ Résolu |
| `@humanwhocodes/config-array` | 0.13.0 | Remplacé par `@eslint/config-array` | `prettier-eslint` → `eslint@8.57.1` → `@humanwhocodes/config-array` | ✅ Résolu |
| `@humanwhocodes/object-schema` | 2.0.3 | Remplacé par `@eslint/object-schema` | `prettier-eslint` → `eslint@8.57.1` → `@humanwhocodes/config-array` → `@humanwhocodes/object-schema` | ✅ Résolu |
| `rimraf` | 3.0.2 | Versions < 4 non supportées | `prettier-eslint` → `eslint@8.57.1` → `flat-cache` → `rimraf@3.0.2` | ✅ Résolu |
| `glob` | 7.2.3 | Vulnérabilités de sécurité connues | `prettier-eslint` → `eslint@8.57.1` → `rimraf@3.0.2` → `glob@7.2.3` | ✅ Résolu |
| `inflight` | 1.0.6 | Fuite mémoire, non maintenu | `prettier-eslint` → `eslint@8.57.1` → `glob@7.2.3` → `inflight@1.0.6` | ✅ Résolu |
| `lodash.isequal` | 4.5.0 | Déprécié, remplacer par `node:util` | `quill@~2.0.3` → `quill-delta@5.1.0` → `lodash.isequal@4.5.0` | ⏳ En attente de Quill |

### 1.2 Détail des chaînes de dépendances

#### ~~`prettier-eslint@~16.4.2`~~ (supprimé — migration effectuée)

Cette dépendance était **la principale source des avertissements**. Elle embarquait une version obsolète d'ESLint (v8) en interne, qui entraînait toute une chaîne de paquets dépréciés :

```
prettier-eslint@16.4.2  [SUPPRIMÉ]
  ├── eslint@8.57.1           ✅ résolu
  │   ├── @humanwhocodes/config-array@0.13.0   ✅ résolu
  │   │   └── @humanwhocodes/object-schema@2.0.3  ✅ résolu
  │   ├── file-entry-cache@6.0.1
  │   │   └── flat-cache@3.2.0
  │   │       └── rimraf@3.0.2               ✅ résolu
  │   │           └── glob@7.2.3             ✅ résolu
  │   │               └── inflight@1.0.6     ✅ résolu
  │   └── @eslint/eslintrc@2.1.4
  │       └── minimatch@3.1.5
  └── @typescript-eslint/parser@6.21.0
      └── @typescript-eslint/typescript-estree@6.21.0
          └── minimatch@9.0.3               ✅ résolu (ReDoS)
```

#### `quill@~2.0.3` (dependency directe)

```
quill@2.0.3
  └── quill-delta@5.1.0
      └── lodash.isequal@4.5.0             ⏳ déprécié — en attente Quill
```

---

## 2. Vulnérabilités de sécurité (`npm audit`)

Avant migration, `npm audit` identifiait **8 vulnérabilités** (1 faible, 7 élevées). Après suppression de `prettier-eslint`, le total est ramené à **4 vulnérabilités** (1 faible, 3 élevées).

### 2.1 Résumé

| Paquet vulnérable | Versions concernées | Sévérité | Type | CVE/Advisory | Origine dans `package.json` | Statut |
|---|---|---|---|---|---|---|
| `minimatch` | 9.0.0 – 9.0.6 | **Haute** | ReDoS (3 vecteurs) | [GHSA-3ppc-4f35-3m26](https://github.com/advisories/GHSA-3ppc-4f35-3m26), [GHSA-7r86-cg39-jmmj](https://github.com/advisories/GHSA-7r86-cg39-jmmj), [GHSA-23c5-xmqv-rm74](https://github.com/advisories/GHSA-23c5-xmqv-rm74) | `prettier-eslint@~16.4.2` | ✅ Résolu |
| `quill` | 2.0.3 | **Faible** | XSS via export HTML | [GHSA-v3m3-f69x-jf25](https://github.com/advisories/GHSA-v3m3-f69x-jf25) | `quill@~2.0.3` (direct) | ⏳ En attente de release |
| `undici` | 7.0.0 – 7.23.0 | **Haute** | Plusieurs vecteurs (WebSocket, HTTP smuggling, DoS) | [GHSA-f269-vfmq-vjvj](https://github.com/advisories/GHSA-f269-vfmq-vjvj), [GHSA-2mjp-6q6p-2qxm](https://github.com/advisories/GHSA-2mjp-6q6p-2qxm), [GHSA-vrm6-8vpv-qv8q](https://github.com/advisories/GHSA-vrm6-8vpv-qv8q), [GHSA-v9p9-hfj2-hcw8](https://github.com/advisories/GHSA-v9p9-hfj2-hcw8), [GHSA-4992-7rv2-5pvq](https://github.com/advisories/GHSA-4992-7rv2-5pvq), [GHSA-phc3-fgpg-7m6h](https://github.com/advisories/GHSA-phc3-fgpg-7m6h) | `@angular/build@~21.2.3` / `@angular-devkit/build-angular@~21.2.3` (dev) | ⏳ En attente de patch Angular |

### 2.2 Détail des vulnérabilités

#### ~~`minimatch` — ReDoS~~ (Haute) — ✅ Résolu

Cette vulnérabilité provenait de `prettier-eslint` qui a été supprimé. Elle n'est plus présente après la migration.

#### `quill@2.0.3` — XSS (Faible)

L'éditeur de texte riche Quill version 2.0.3 est vulnérable à une attaque XSS via la fonctionnalité d'export HTML. Cette dépendance est **directe** et utilisée en production. La correction nécessite une mise à jour vers une version corrigée.

#### `undici` — Multiple (Haute)

Chaîne de dépendance :
```
@angular/build@21.2.3           (devDependency)
  └── undici@7.22.0   ← versions 7.0.0–7.23.0 vulnérables
@angular-devkit/build-angular@21.2.3
  └── (dépend de @angular/build)
```

Six vulnérabilités affectant le client HTTP `undici` (inclus dans le système de build Angular 21) :
- Dépassement d'entier dans le parser WebSocket (64 bits)
- HTTP Request/Response Smuggling
- Consommation mémoire non bornée dans WebSocket (décompression)
- Exception non gérée dans le client WebSocket
- Injection CRLF via l'option `upgrade`
- Consommation mémoire non bornée dans `DeduplicationHandler`

Ces vulnérabilités sont **uniquement en contexte de build/développement** et n'affectent pas le bundle de production final.

---

## 3. Solutions proposées

### 3.1 Problèmes résolubles immédiatement

#### 🔴 Priorité haute — `prettier-eslint` (source de la plupart des avertissements)

**Problème** : `prettier-eslint@~16.4.2` importe une version obsolète d'ESLint (v8.x) avec toutes ses dépendances dépréciées.

**Solutions envisageables** :

| Option | Action | Impact |
|---|---|---|
| **A** | Passer à `prettier-eslint@16.3.0` (version qui corrige `minimatch`) | ⚠️ Changement cassant selon npm audit ; risque de régression dans la configuration Prettier+ESLint |
| **B** | Supprimer `prettier-eslint` et `eslint-plugin-prettier`, utiliser uniquement `prettier` et `eslint` séparément | ✅ Élimine tous les avertissements liés à ce paquet ; nécessite adaptation du workflow |
| **C** | Remplacer par `@prettier/eslint` si disponible | ⬜ À évaluer selon disponibilité d'une version compatible |

**Recommandation** : l'option **B** est préférable à long terme. La configuration ESLint actuelle du projet utilise déjà `eslint-config-prettier` (qui désactive les règles de formatage ESLint conflictuelles avec Prettier), ce qui est le pattern recommandé par l'équipe Prettier. Supprimer `prettier-eslint` et `eslint-plugin-prettier` simplifierait la chaîne d'outillage.

Pour la migration :
1. Supprimer `prettier-eslint` et `eslint-plugin-prettier` du `package.json`
2. Conserver `prettier`, `eslint-config-prettier`, et `prettier-plugin-organize-imports`
3. Mettre à jour `eslint.config.js` pour retirer les règles Prettier-as-ESLint-rule
4. Exécuter `prettier` et `eslint` de façon indépendante dans les scripts `clean` / CI

#### 🔴 Priorité haute — `quill@2.0.3`

**Problème** : XSS via l'export HTML + dépendance `lodash.isequal` dépréciée.

**Solutions** :

| Option | Action | Impact |
|---|---|---|
| **A** | Attendre et passer à `quill@2.0.4+` quand la correction sera publiée | ✅ Minimal ; à surveiller sur [le changelog Quill](https://github.com/slab/quill/releases) |
| **B** | Utiliser `ngx-quill` avec une version compatible corrigée | ⬜ Implique une migration de l'intégration Angular |
| **C** | Remplacer Quill par TipTap ou un autre éditeur riche sans ces dépendances | ⬜ Changement structurel important |

**Recommandation** : **Option A** dans l'immédiat (surveiller la sortie d'une version corrigée de Quill). La vulnérabilité XSS est de sévérité faible et n'est déclenchable que si l'export HTML Quill est utilisé dans l'application.

#### 🟡 Priorité moyenne — `undici` via `@angular/build`

**Problème** : `@angular/build@21.2.3` embarque `undici@7.22.0` avec des vulnérabilités connues.

**Solutions** :

| Option | Action | Impact |
|---|---|---|
| **A** | Attendre la mise à jour de `@angular/build` vers une version corrigée (≥ 21.2.4 ou patch Angular) | ✅ Minimal ; les vulnérabilités sont côté build uniquement |
| **B** | Rétrograder vers `@angular/build@20.3.21` (version sans undici vulnérable) | ❌ Changement cassant, perte des fonctionnalités Angular 21 |

**Recommandation** : **Option A**. Ces vulnérabilités n'affectent pas le bundle de production final. Surveiller les releases Angular 21.2.x pour un patch.

### 3.2 Récapitulatif des actions

| Priorité | Paquet direct | Action recommandée | Statut |
|---|---|---|---|
| 🔴 Haute | `prettier-eslint@~16.4.2` + `eslint-plugin-prettier@~5.5.5` | Supprimés — migration vers Prettier + ESLint séparés effectuée | ✅ Résolu |
| 🔴 Haute | `quill@~2.0.3` | Mettre à jour vers `quill@2.0.4+` dès disponibilité | ⏳ En attente de release |
| 🟡 Moyenne | `@angular/build@~21.2.3` | Mettre à jour dès publication d'un patch Angular 21.2.x | ⏳ En attente de release |

---

## 4. Rôles des paquets npm par fonctionnalité

### 4.1 Framework Angular (Core)

Ces paquets constituent le socle du framework Angular. Ils sont tous versionnés à `~21.2.x` pour garantir la cohérence.

| Paquet | Type | Rôle |
|---|---|---|
| `@angular/core` | dep | Noyau Angular : décorateurs, DI, cycle de vie des composants |
| `@angular/common` | dep | Utilitaires communs : pipes (`DatePipe`, `AsyncPipe`…), directives (`NgIf`, `NgFor`…) |
| `@angular/compiler` | dep | Compilateur Angular (JIT) pour les templates |
| `@angular/compiler-cli` | dev | Compilateur Angular (AOT) utilisé lors du build |
| `@angular/platform-browser` | dep | Pont entre Angular et le DOM du navigateur |
| `@angular/platform-browser-dynamic` | dep | Compilation JIT dans le navigateur (bootstrap de l'application) |
| `@angular/router` | dep | Système de routage SPA avec lazy loading |
| `@angular/forms` | dep | Gestion des formulaires réactifs (`FormBuilder`, `FormGroup`…) |
| `@angular/animations` | dep | Système d'animations Angular |
| `@angular/cdk` | dep | Angular Component Dev Kit : blocs de base pour composants UI (overlay, drag & drop, a11y…) |
| `@angular/service-worker` | dep | Support PWA : gestion du Service Worker pour le mode hors-ligne et le cache |
| `@angular/language-service` | dev | Service de langage Angular pour l'autocomplétion et la vérification dans les IDE (VS Code) |

### 4.2 Build & CLI Angular

Outils de compilation, de génération de code et de développement.

| Paquet | Type | Rôle |
|---|---|---|
| `@angular/cli` | dev | Interface en ligne de commande Angular (`ng serve`, `ng build`, `ng generate`…) |
| `@angular/build` | dev | Système de build Angular basé sur esbuild (remplace Webpack) |
| `@angular-devkit/build-angular` | dev | Builders Angular DevKit : gestion des configurations de build, test, serve |
| `@angular-devkit/core` | dev | Utilitaires internes du DevKit Angular (système de fichiers virtuel, workspace…) |
| `@angular-devkit/schematics` | dev | Moteur de schematics pour la génération/modification de code via `ng generate` |
| `ng-packagr` | dev | Outil de packaging pour les bibliothèques Angular (génère le format APF) — utilisé pour construire la lib `bia-ng` |

### 4.3 Gestion d'état (NgRx)

Implémentation du pattern Redux pour la gestion d'état applicatif.

| Paquet | Type | Rôle |
|---|---|---|
| `@ngrx/store` | dep | Store Redux central : state immutable, reducers, selectors |
| `@ngrx/effects` | dep | Gestion des effets secondaires (appels HTTP, WebSocket…) en réaction aux actions du store |
| `@ngrx/entity` | dep | Adaptateur pour la gestion des collections d'entités dans le store |
| `@ngrx/operators` | dep | Opérateurs RxJS spécifiques à NgRx |
| `@ngrx/store-devtools` | dev | Intégration avec Redux DevTools pour le débogage du store en développement |

### 4.4 Interface utilisateur (PrimeNG / PrimeFaces)

Suite de composants UI et de styles.

| Paquet | Type | Rôle |
|---|---|---|
| `primeng` | dep | Bibliothèque de composants UI Angular (tables, dialogues, formulaires, menus…) |
| `primeflex` | dep | Framework CSS utilitaire (flexbox/grid) pour la mise en page responsive |
| `primeicons` | dep | Bibliothèque d'icônes SVG utilisée par PrimeNG |
| `@primeuix/themes` | dep | Système de thèmes pour PrimeNG v4+ (remplace les anciens thèmes CSS) |
| `quill` | dep | Éditeur de texte riche (WYSIWYG), intégré dans le composant `p-editor` de PrimeNG |

### 4.5 Authentification & Sécurité

| Paquet | Type | Rôle |
|---|---|---|
| `keycloak-angular` | dep | Intégration de Keycloak dans Angular : intercepteur HTTP, guard de route, service d'authentification |
| `keycloak-js` | dep | Client JavaScript officiel de Keycloak (OpenID Connect / OAuth2) |
| `jwt-decode` | dep | Décodage (sans vérification) des tokens JWT pour en extraire les claims côté client |

### 4.6 Communication temps réel

| Paquet | Type | Rôle |
|---|---|---|
| `@microsoft/signalr` | dep | Client SignalR Microsoft pour les communications temps réel (WebSocket/SSE/Long-polling) avec le backend ASP.NET Core |

### 4.7 Internationalisation (i18n)

| Paquet | Type | Rôle |
|---|---|---|
| `@ngx-translate/core` | dep | Bibliothèque de traduction/localisation pour Angular : pipe `translate`, service `TranslateService`, chargement de fichiers JSON de traduction |

### 4.8 Gestion des données & Dates

| Paquet | Type | Rôle |
|---|---|---|
| `date-fns` | dep | Bibliothèque de manipulation de dates (parsing, formatage, calculs) — alternative légère à Moment.js |
| `deepmerge` | dep | Fusion profonde (deep merge) d'objets JavaScript, utilisée notamment pour la configuration BIA |
| `dexie` | dep | Wrapper promis/async sur l'API IndexedDB du navigateur, pour la persistance locale des données (mode hors-ligne) |

### 4.9 Gestion de fichiers

| Paquet | Type | Rôle |
|---|---|---|
| `file-saver` | dep | Téléchargement de fichiers côté client (génération de fichiers CSV, Excel, PDF…) |
| `@types/file-saver` | dep | Définitions TypeScript pour `file-saver` |
| `papaparse` | dep | Parsing et génération de fichiers CSV performants |
| `@types/papaparse` | dev | Définitions TypeScript pour `papaparse` |

### 4.10 Programmation réactive

| Paquet | Type | Rôle |
|---|---|---|
| `rxjs` | dep | Bibliothèque de programmation réactive (Observables, opérateurs) — fondation de l'architecture Angular |
| `zone.js` | dep | Zone.js : gestion des zones d'exécution JavaScript, utilisé par Angular pour la détection automatique des changements |

### 4.11 TypeScript & Transpilation

| Paquet | Type | Rôle |
|---|---|---|
| `typescript` | dev | Compilateur TypeScript |
| `ts-node` | dev | Exécution de fichiers TypeScript directement dans Node.js (utilisé pour les scripts de configuration) |
| `tslib` | dev | Bibliothèque d'helpers TypeScript (évite la duplication du code généré par `tslib` dans chaque fichier) |
| `@types/node` | dev | Définitions TypeScript pour les APIs Node.js (utilisées dans les scripts de build) |

### 4.12 Qualité du code — Linting (ESLint)

| Paquet | Type | Rôle |
|---|---|---|
| `eslint` | dev | Moteur de linting JavaScript/TypeScript |
| `@eslint/js` | dev | Configuration ESLint recommandée pour JavaScript |
| `@angular-eslint/builder` | dev | Builder ESLint pour Angular (intégration avec `ng lint`) |
| `@angular-eslint/eslint-plugin` | dev | Règles ESLint spécifiques aux composants Angular (TypeScript) |
| `@angular-eslint/eslint-plugin-template` | dev | Règles ESLint pour les templates Angular HTML |
| `@angular-eslint/template-parser` | dev | Parser ESLint pour les templates Angular |
| `@angular-eslint/schematics` | dev | Schematics pour configurer angular-eslint via `ng add` |
| `angular-eslint` | dev | Meta-paquet regroupant tous les paquets `@angular-eslint/*` |
| `@typescript-eslint/eslint-plugin` | dev | Règles ESLint spécifiques à TypeScript |
| `@typescript-eslint/parser` | dev | Parser TypeScript pour ESLint |
| `typescript-eslint` | dev | Meta-paquet regroupant `@typescript-eslint/parser` et `@typescript-eslint/eslint-plugin` |

### 4.13 Qualité du code — Formatage (Prettier)

| Paquet | Type | Rôle |
|---|---|---|
| `prettier` | dev | Formateur de code automatique (TypeScript, HTML, CSS, JSON…) |
| `eslint-config-prettier` | dev | Configuration ESLint désactivant les règles conflictuelles avec Prettier (ESLint et Prettier fonctionnent désormais indépendamment) |
| `prettier-plugin-organize-imports` | dev | Plugin Prettier pour trier/organiser automatiquement les imports TypeScript |

### 4.14 Résumé visuel

```
┌─────────────────────────────────────────────────────────────────┐
│                    APPLICATION BIADemo Angular                  │
├──────────────────────────────┬──────────────────────────────────┤
│     RUNTIME (production)     │      DÉVELOPPEMENT UNIQUEMENT    │
├──────────────────────────────┼──────────────────────────────────┤
│ 🅰️  Angular Core (12 pkg)    │ 🔨 Build / CLI (6 pkg)          │
│ 📦 NgRx / State (4 pkg)      │ 🔍 ESLint / Lint (11 pkg)       │
│ 🎨 PrimeNG / UI (5 pkg)      │ ✨ Prettier / Format (5 pkg)    │
│ 🔐 Auth / Keycloak (3 pkg)   │ 📘 TypeScript (4 pkg)           │
│ ⚡ SignalR / RT (1 pkg)       │                                  │
│ 🌍 i18n / Translate (1 pkg)  │                                  │
│ 📅 Données / Dates (3 pkg)   │                                  │
│ 📁 Fichiers (4 pkg)          │                                  │
│ 🔄 RxJS / Zones (2 pkg)      │                                  │
└──────────────────────────────┴──────────────────────────────────┘
```
