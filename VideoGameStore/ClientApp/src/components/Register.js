import React, { Component } from 'react';
import './NavMenu.css';


export class Register extends Component {
    static displayName = Register.name;

    constructor(props) {
        super(props);

        this.state = {
            username: '',
            password: '',
            name: '',
            surname: '',
            email: '',
            phone: '',
            referal: '',
            date: new Date().toLocaleDateString()
        };
    }

    handleInputChange = (event) => {
        const { name, value } = event.target;
        this.setState({ [name]: value });
    }

    handleSubmit = (event) => {
        event.preventDefault();
    }

    render() {
        return (
            <div>
                <h2>Registration</h2>
                <form onSubmit={this.handleSubmit}>
                        <div>
                            <label>Username:</label><br />
                            <input
                                type="text"
                                name="username"
                                value={this.state.username}
                                onChange={this.handleInputChange}
                            />
                        </div><br />

                        <div>
                            <label>Password:</label><br />
                            <input
                                type="password"
                                name="password"
                                value={this.state.password}
                                onChange={this.handleInputChange}
                            />
                        </div><br />

                        <div>
                            <label>Name:</label><br />
                            <input
                                type="name"
                                name="text"
                                value={this.state.name}
                                onChange={this.handleInputChange}
                            />
                        </div><br />

                        <div>
                            <label>Surname:</label><br />
                            <input
                                type="surname"
                                name="text"
                                value={this.state.surname}
                                onChange={this.handleInputChange}
                            />
                        </div><br />

                        <div>
                            <label>Email:</label><br />
                            <input
                                type="email"
                                name="email"
                                value={this.state.email}
                                onChange={this.handleInputChange}
                            />
                        </div><br />

                        <div>
                            <label>Phone number:</label><br />
                            <input
                                type="phone"
                                name="tel"
                                value={this.state.phone}
                                onChange={this.handleInputChange}
                            />
                        </div><br />

                        <div>
                            <label>Referral code:</label><br />
                            <input
                                type="text"
                                name="referal"
                                value={this.state.referal}
                                onChange={this.handleInputChange}
                            />
                        </div><br />

                    <div>
                        <button type="submit">Register</button>
                    </div>
                </form>
            </div>
        );
    }
}
