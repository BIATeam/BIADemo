const uselightToken = false;

const BUTTON_LABELS = {
  BiaLogin: "BIA Login",
  ApiLogin: "API Login",
  KeycloakLogin: "Keycloak Login",
};

class KeycloakParam {
  constructor(isActive, idpHint, relativeUrl, baseUrl, clientId) {
    this.isActive = isActive;
    this.idpHint = idpHint;
    this.urlToken = baseUrl + relativeUrl;
    this.urlAuth = (baseUrl + relativeUrl).replace(/\/token$/, "/auth");
    this.clientId = clientId;
  }
}

let keycloakParam;
let sal_button, sal_light_button, sal_logout_button;

const currentUrl = window.location.origin + window.location.pathname;

/**
 * Inject the Login button to the page.
 */
function injectLoginButton() {
  // inject the button
  const div = document.createElement("div");
  let html = `
    <button id="sal-button" class="sal-button" type="button"></button>
    <button id="sal-light-button" class="sal-button sal-light-button" type="button"></button>
    <button id="sal-logout-button" class="sal-button sal-logout-button" type="button">X</button>
`;
  if (keycloakParam.isActive === true) {
    html +=
      '<button id="sal-keycloak-button" class="sal-button sal-light-button sal-keycloak-button" type="button"></button>';
  }
  div.innerHTML = html;
  document.body.appendChild(div);

  sal_button = document.getElementById("sal-button");
  setButtonState("ready", sal_button, BUTTON_LABELS.BiaLogin);
  sal_button.onclick = function () {
    setButtonState("ready", sal_light_button, BUTTON_LABELS.ApiLogin);
    initLogIn("?lightToken=false", sal_button, BUTTON_LABELS.BiaLogin);
  };

  sal_light_button = document.getElementById("sal-light-button");
  setButtonState("ready", sal_light_button, BUTTON_LABELS.ApiLogin);
  sal_light_button.onclick = function () {
    setButtonState("ready", sal_button, BUTTON_LABELS.BiaLogin);
    initLogIn("?lightToken=true", sal_light_button, BUTTON_LABELS.ApiLogin);
  };

  if (keycloakParam.isActive === true) {
    sal_keycloak_button = document.getElementById("sal-keycloak-button");
    setButtonState("ready", sal_keycloak_button, BUTTON_LABELS.KeycloakLogin);
    sal_keycloak_button.onclick = function () {
      setButtonState("ready", sal_button, BUTTON_LABELS.BiaLogin);
      setButtonState("ready", sal_keycloak_button, BUTTON_LABELS.ApiLogin);
      initLogIn("", sal_keycloak_button, BUTTON_LABELS.KeycloakLogin);
    };
  }

  sal_logout_button = document.getElementById("sal-logout-button");
  sal_logout_button.onclick = function () {
    setButtonState("ready", sal_button, BUTTON_LABELS.BiaLogin);
    setButtonState("ready", sal_light_button, BUTTON_LABELS.ApiLogin);
    if (keycloakParam.isActive === true) {
      setButtonState("ready", sal_keycloak_button, BUTTON_LABELS.KeycloakLogin);
    }
    logOut();
  };
}

function logOut() {
  const authorizeLockedButton = document.querySelector(".btn.authorize.locked");

  if (authorizeLockedButton) {
    authorizeLockedButton.click();

    setTimeout(() => {
      const logoutButton = document.querySelector(
        '.auth-container button[aria-label="Remove authorization"]'
      );
      const closeButton = document.querySelector(".auth-container .btn-done");

      logoutButton.click();
      closeButton.click();
    }, 100);
  }
}

/**
 * Perform the login.
 */
function logIn(param, button, text, accessToken, useAccessToken = false) {
  waitForElement(".btn.authorize", function () {
    logOut();
    setButtonState("loading", button, text);

    const inputAuth = document.querySelector(
      ".auth-container input[type='text']"
    );
    if (!inputAuth) {
      document.querySelector(".btn.authorize").click();
    }

    if (useAccessToken) {
      setJwt(accessToken, button, text);
    } else {
      let url = `../api/Auth/login${param}`;
      if (uselightToken === false && param === "?lightToken=true") {
        url = `../api/Auth/token`;
      }
      console.log(url);
      fetch(url, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          ...(accessToken && { Authorization: `Bearer ${accessToken}` }),
        },
      })
        .then((response) => {
          if (!response.ok) {
            throw new Error("Network response was not ok");
          }
          return response.json();
        })
        .then((data) => {
          const token = data.token || data;
          setJwt(token, button, text);
        })
        .catch((error) => {
          console.error("Login error:", error);
          setButtonState("error", button, text);
        });
    }
  });
}

function setNativeValue(element, value) {
  const valueSetter = Object.getOwnPropertyDescriptor(element, "value").set;
  const prototype = Object.getPrototypeOf(element);
  const prototypeValueSetter = Object.getOwnPropertyDescriptor(
    prototype,
    "value"
  ).set;

  if (valueSetter && valueSetter !== prototypeValueSetter) {
    prototypeValueSetter.call(element, value);
  } else {
    valueSetter.call(element, value);
  }
}

function setJwt(token, button, text) {
  waitForElement(".auth-container input[type='text']", function () {
    console.log("setJwt ", token);
    const inputAuth = document.querySelector(
      ".auth-container input[type='text']"
    );
    setNativeValue(inputAuth, token);
    inputAuth.dispatchEvent(new Event("input", { bubbles: true }));
    document.querySelector(".btn.modal-btn.auth.authorize.button").click();
    document.querySelector(".btn.modal-btn.auth.btn-done.button").click();
    setButtonState("logged", button, text);
  });
}

/**
 * Set the state of the button.
 *
 * @param {String} state
 */
function setButtonState(state, button, text) {
  if (button) {
    button.classList.remove("sal-error");
    button.classList.remove("sal-spinner");
    button.classList.remove("sal-ready");
    button.classList.remove("sal-logged");
    switch (state) {
      case "ready":
        button.classList.add("sal-ready");
        button.innerHTML = text;
        break;
      case "logged":
        button.classList.add("sal-logged");
        button.innerHTML = text;
        break;
      case "loading":
        button.classList.add("sal-spinner");
        button.innerHTML = "";
        break;
      case "error":
        button.classList.add("sal-error");
        button.innerHTML = "Try Again";
        break;
    }
  }
}

function callTokenEndpoint(keycloakParam, text) {
  const params = new URLSearchParams();
  params.append("client_id", keycloakParam.clientId);
  params.append("response_type", "code");
  params.append("scope", "openid");
  params.append(
    "redirect_uri",
    currentUrl + "?biatext=" + encodeURIComponent(text)
  );
  params.append("kc_idp_hint", keycloakParam.idpHint);

  const fullUrl = keycloakParam.urlAuth + "?" + params.toString();

  window.location.href = fullUrl;
}

function handleRedirect() {
  const urlParams = new URLSearchParams(window.location.search);
  const authCode = urlParams.get("code");
  const text = urlParams.get("biatext");

  if (authCode) {
    exchangeCodeForToken(authCode, text);
  }
}

/**
 * Exchange authorization code for a token.
 */
function exchangeCodeForToken(authCode, text) {
  const formData = new URLSearchParams({
    client_id: keycloakParam.clientId,
    grant_type: "authorization_code",
    code: authCode,
    redirect_uri: `${currentUrl}?biatext=${encodeURIComponent(text)}`,
  });

  fetch(keycloakParam.urlToken, {
    method: "POST",
    headers: { "Content-Type": "application/x-www-form-urlencoded" },
    body: formData.toString(),
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error("Network response was not ok");
      }
      return response.json();
    })
    .then((data) => {
      const token = data.access_token;

      if (text === BUTTON_LABELS.ApiLogin) {
        logIn("?lightToken=true", sal_light_button, text, token);
      } else if (text === BUTTON_LABELS.BiaLogin) {
        logIn("?lightToken=false", sal_button, BUTTON_LABELS.BiaLogin, token);
      } else {
        logIn(
          "",
          sal_keycloak_button,
          BUTTON_LABELS.KeycloakLogin,
          token,
          true
        );
      }
    })
    .catch((error) => {
      console.error("Error exchanging code:", error);
    });
}

function waitForElement(selector, callback, timeout = 5000) {
  const interval = 100;
  let elapsedTime = 0;

  const checkExist = setInterval(() => {
    if (document.querySelectorAll(selector).length > 0) {
      clearInterval(checkExist);
      callback(document.querySelector(selector));
    }

    elapsedTime += interval;
    if (elapsedTime >= timeout) {
      clearInterval(checkExist);
      console.error(`Element ${selector} not found within ${timeout}ms`);
    }
  }, interval);
}

function initLogIn(param, button, text) {
  setButtonState("loading", button, text);
  if (keycloakParam.isActive === true) {
    callTokenEndpoint(keycloakParam, text);
  } else {
    logIn(param, button, text, null);
  }
}

/**
 * Fetch Keycloak parameters from the server.
 */
function fetchKeycloakParam() {
  return fetch("../api/AppSettings")
    .then((response) => {
      if (!response.ok) {
        throw new Error("Failed to fetch Keycloak parameters");
      }
      return response.json();
    })
    .then((appSetting) => {
      return new KeycloakParam(
        appSetting.keycloak.isActive,
        appSetting.keycloak.configuration.idpHint,
        appSetting.keycloak.api.tokenConf.relativeUrl,
        appSetting.keycloak.baseUrl,
        appSetting.keycloak.api.tokenConf.clientId
      );
    })
    .catch((error) => {
      console.error("Error fetching Keycloak parameters:", error);
    });
}

/**
 * Initialize the application.
 */
function init() {
  fetchKeycloakParam()
    .then((param) => {
      keycloakParam = param;
      if (keycloakParam) {
        injectLoginButton();
        handleRedirect();
      }
    })
    .catch((error) => {
      console.error("Initialization error:", error);
    });
}

if (
  document.readyState === "complete" ||
  (document.readyState !== "loading" && !document.documentElement.doScroll)
) {
  init();
} else {
  document.addEventListener("DOMContentLoaded", function () {
    init();
  });
}
