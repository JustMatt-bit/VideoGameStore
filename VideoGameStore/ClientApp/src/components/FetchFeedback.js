import React, { Component } from 'react';

export class FetchFeedback extends Component {
    //static displayName = FetchFeedback.name;

    constructor(props) {
        super(props);
        this.state = { feedback: [], loading: true };
    }


    componentDidMount() {
        this.populateVideoGameData();
    }

    static renderFeedbackTable(feedback) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Tekstas</th>
                        <th>Data</th>
                        <th>Vid. įvertinimas</th>
                        <th>Įvertinimų kiekis</th>
                        <th>Netinkamas</th>
                        <th>Vartotojas</th>
                    </tr>
                </thead>
                <tbody>
                    {feedback.map(feedbackItem =>
                        <tr key={feedbackItem.id}>
                            <td>{feedbackItem.text}</td>
                            <td>{feedbackItem.date.split("T")[0]}</td>
                            <td>{feedbackItem.rating}</td>
                            <td>{feedbackItem.rating_count}</td>
                            <td>{feedbackItem.is_flagged ? "Taip" : "Ne"}</td>
                            <td>{feedbackItem.account_name}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : FetchFeedback.renderFeedbackTable(this.state.feedback);

        return (
            <div>
                <h1 id="tabelLabel" >Produktai</h1>
                <p>Komponentas (puslapis), rodantis MySQL integraciją.</p>
                {contents}
            </div>
        );
    }

    async populateVideoGameData() {
        //const productId = this.props.productId;
        const productId = 1;
        const response = await fetch(`api/feedback/${productId}`);
        const data = await response.json();
        this.setState({ feedback: data, loading: false });
    }
}