// @ts-check
const eslint = require("@eslint/js");
const { defineConfig } = require("eslint/config");
const tseslint = require("typescript-eslint");
const angular = require("angular-eslint");
const prettierRecommended = require("eslint-plugin-prettier/recommended");
const eslintConfigPrettier = require("eslint-config-prettier");

module.exports = defineConfig([
  // Global ignores
  { ignores: ["projects/**/*"] },

  // TypeScript files
  {
    files: ["**/*.ts"],
    extends: [
      eslint.configs.recommended,
      ...tseslint.configs.recommended,
      ...angular.configs.tsRecommended,
      prettierRecommended,
      eslintConfigPrettier,
    ],
    processor: angular.processInlineTemplates,
    languageOptions: {
      parserOptions: {
        project: ["tsconfig.json"],
      },
    },
    rules: {
      "prettier/prettier": "error",
      "@typescript-eslint/no-namespace": "off",
      "@typescript-eslint/no-explicit-any": "off",
      "@typescript-eslint/no-unused-vars": "error",
      "@typescript-eslint/naming-convention": "error",
      "@typescript-eslint/no-deprecated": "warn",
      "@angular-eslint/directive-selector": [
        "error",
        {
          type: "attribute",
          prefix: "app",
          style: "camelCase",
        },
      ],
      "@angular-eslint/component-selector": [
        "error",
        {
          type: "element",
          prefix: "app",
          style: "kebab-case",
        },
      ],
      "eqeqeq": ["error", "always", { null: "ignore" }],
      "@angular-eslint/prefer-inject": "off",
    },
  },

  // BIA-shared and BIA-features: override selectors to use "bia" prefix
  {
    files: [
      "src/app/features/bia-features/**/*.ts",
      "src/app/shared/bia-shared/**/*.ts",
    ],
    rules: {
      "@angular-eslint/component-selector": [
        "error",
        {
          type: "element",
          prefix: "bia",
          style: "kebab-case",
        },
      ],
      "@angular-eslint/directive-selector": [
        "error",
        {
          type: "attribute",
          prefix: "bia",
          style: "camelCase",
        },
      ],
      "eqeqeq": ["error", "always", { null: "ignore" }],
    },
  },

  // HTML template files
  {
    files: ["**/*.html"],
    ignores: ["**/*inline-template-*.component.html"],
    extends: [
      ...angular.configs.templateRecommended,
      ...angular.configs.templateAccessibility,
      prettierRecommended,
      eslintConfigPrettier,
    ],
    rules: {
      "@angular-eslint/template/click-events-have-key-events": "off",
      "@angular-eslint/template/interactive-supports-focus": "off",
      "@angular-eslint/template/elements-content": "off",
      "@angular-eslint/template/label-has-associated-control": "off",
      "@angular-eslint/template/no-autofocus": "off",
      "prettier/prettier": ["error", { parser: "angular" }],
      "eqeqeq": ["error", "always", { null: "ignore" }],
    },
  },
]);
