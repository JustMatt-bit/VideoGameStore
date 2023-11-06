import React, { Component } from 'react';
import './NavMenu.css';


export class ActivateDiscount extends Component {
    static displayName = ActivateDiscount.name;

    constructor(props) {
        super(props);

        this.state = {
            code: "",
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
                <h2>Activate discount code</h2>
                <form onSubmit={this.handleSubmit}>
                    <div>
                        <label>Discount code:</label><br />
                        <input
                            type="text"
                            name="code"
                            value={this.state.username}
                            onChange={this.handleInputChange}
                        />
                    </div><br />

                    
                    <div>
                        <button type="submit">Activate</button>
                    </div>
                </form>
            </div>
        );
    }
}
