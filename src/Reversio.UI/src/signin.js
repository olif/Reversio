export class Signin {
    constructor(signInCallback) {
        this.element = this.createElement();
        this.signInCallback = signInCallback;
    }

    createElement() {
        let html = `<form class="signin">
                        <label>
                            Username:
                            <input type="text" class="username" />
                        </label>
                        <button type="submit" class="login-button">Sign in</button>
                    </form>`;

        let elem = $(html);
        $('body').on('submit', '.signin', (e) => this.onSubmit(e))
        return $(html);
    }

    onSubmit(e) {
        e.preventDefault();
        let userName = $(e.target).find('.username').val();
        if(this.signInCallback != null) {
            this.signInCallback(userName);
        }
    }

    appendToElement(elem) {
        elem.append(this.element);
    }
}