var callback = function () {
    injectLoginButton();
};


/**
 * Inject the Login button to the page.
 */
function injectLoginButton() {
    // inject the button
    var html = document.createElement('div')
    html.innerHTML = '<button id="sal-button" type="button"></button>'
    document.body.appendChild(html)

    button = document.getElementById('sal-button')
    setButtonState('ready')

    // register click handler
    button.onclick = function () {
        logIn()
    }
}

/**
 * Perform the login.
 */
function logIn() {
    setButtonState('loading')
    var inputAuth = $('.auth-container :input[type=text]')[0];
    if (inputAuth == undefined) {
        $(".btn.authorize").click();
    }
    var xhr = new XMLHttpRequest()
    xhr.open('GET', '../api/Auth/login', true)
    xhr.setRequestHeader('Content-type', 'application/json')

    xhr.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            // find the auth token
            var response = JSON.parse(this.responseText)
            var token = response.token

            setJwt(token);
            $('.btn.modal-btn.auth.authorize.button').click()
            $('.btn.modal-btn.auth.btn-done.button').click()
            setButtonState('ready')
        } else if (this.readyState == 4) {
            setButtonState('error')
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
function setButtonState(state) {
    switch (state) {
        case 'ready':
            button.classList.remove('sal-error')
            button.innerHTML = 'BIA Log In'
            break
        case 'loading':
            button.classList.remove('sal-error')
            button.innerHTML = '<div id="sal-spinner"></div>'
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