var callback = function () {
    injectLoginButton();
};

var sal_button;
var sal_light_button;
var sal_logout_button;

/**
 * Inject the Login button to the page.
 */
function injectLoginButton() {
    // inject the button
    var html = document.createElement('div')
    html.innerHTML = '<button id="sal-button" class="sal-button" type="button"></button> <button id="sal-light-button" class="sal-button sal-light-button" type="button"></button> <button id="sal-logout-button" class="sal-button sal-logout-button" type="button">X</button>'
    document.body.appendChild(html)

    sal_button = document.getElementById('sal-button');
    setButtonState('ready', sal_button, 'BIA Login');
    sal_button.onclick = function () {
        setButtonState('ready', sal_light_button, 'API Login');
        logIn('?lightToken=false', sal_button, 'BIA Login');
    }

    sal_light_button = document.getElementById('sal-light-button');
    setButtonState('ready', sal_light_button, 'API Login');
    sal_light_button.onclick = function () {
        setButtonState('ready', sal_button, 'BIA Login');
        logIn('?lightToken=true', sal_light_button, 'API Login');
    }

    sal_logout_button = document.getElementById('sal-logout-button');
    sal_logout_button.onclick = function () {
        setButtonState('ready', sal_button, 'BIA Login');
        setButtonState('ready', sal_light_button, 'API Login');
        logOut();
    }
}

function logOut() {
    var inputAuth = $('.auth-container :input[type=text]')[0];
    if (inputAuth == undefined) {
        $(".btn.authorize").click();
    }
    $('.btn.modal-btn.auth.button').click()
    $('.btn.modal-btn.auth.btn-done.button').click()
}

/**
 * Perform the login.
 */
function logIn(param, button, text) {
    logOut();
    setButtonState('loading', button, text)
    var inputAuth = $('.auth-container :input[type=text]')[0];
    if (inputAuth == undefined) {
        $(".btn.authorize").click();
    }
    var xhr = new XMLHttpRequest()
    xhr.open('GET', '../api/Auth/login'+ param, true)
    xhr.setRequestHeader('Content-type', 'application/json')

    xhr.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            // find the auth token
            var response = JSON.parse(this.responseText)
            var token = response.token

            setJwt(token);
            $('.btn.modal-btn.auth.authorize.button').click()
            $('.btn.modal-btn.auth.btn-done.button').click()
            setButtonState('logged', button, text)
        } else if (this.readyState == 4) {
            setButtonState('error', button, text)
        }
    }

    xhr.send()
}

function setNativeValue(element, value) {
    const valueSetter = Object.getOwnPropertyDescriptor(element, 'value').set;
    const prototype = Object.getPrototypeOf(element);
    const prototypeValueSetter = Object.getOwnPropertyDescriptor(prototype, 'value').set;

    if (valueSetter && valueSetter !== prototypeValueSetter) {
        prototypeValueSetter.call(element, value);
    } else {
        valueSetter.call(element, value);
    }
}

function setJwt(key) {
    console.log("setJwt ", key);
    var inputAuth = $('.auth-container :input[type=text]')[0];
    setNativeValue(inputAuth, key);
    inputAuth.dispatchEvent(new Event('input', { bubbles: true }));
}

/**
 * Set the state of the button.
 *
 * @param {String} state
 */
function setButtonState(state, button, text) {
    button.classList.remove('sal-error')
    button.classList.remove('sal-spinner')
    button.classList.remove('sal-ready')
    button.classList.remove('sal-logged')
    switch (state) {
        case 'ready':
            button.classList.add('sal-ready')
            button.innerHTML = text;
            break
        case 'logged':
            button.classList.add('sal-logged')
            button.innerHTML = text;
            break
        case 'loading':
            button.classList.add('sal-spinner')
            button.innerHTML = ''
            break
        case 'error':
            button.classList.add('sal-error')
            button.innerHTML = 'Try Again'
            break
    }
}

if (document.readyState === "complete" || (document.readyState !== "loading" && !document.documentElement.doScroll)) {
    callback();
} else {
    document.addEventListener("DOMContentLoaded", callback);
}