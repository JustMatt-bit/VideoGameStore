import React, { Component } from 'react';

export class Shipping extends Component {
    static displayName = Shipping.name;

    constructor(props) {
        super(props);

        this.state = { };
    }

    render() {
        return (
            <div>
                <h2>
                    Shipping cost will be calculated here
                </h2>
            </div>
        );
    }


}