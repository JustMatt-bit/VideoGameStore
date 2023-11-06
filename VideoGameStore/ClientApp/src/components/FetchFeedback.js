import React, { Component, useState } from 'react';
import "./FetchFeedback.css";


const StarRating = () => {

    // NENAUDOTI USE STATES
    const [rating, setRating] = useState(0);
    const [hover, setHover] = useState(0);
    return (
        <div className="star-rating">
            {[...Array(5)].map((star, index) => {
                index += 1;
                return (
                    <button
                        type="button"
                        key={index}
                        className={index <= (hover || rating) ? "on" : "off"}
                        onClick={() => setRating(index)}
                        onMouseEnter={() => setHover(index)}
                        onMouseLeave={() => setHover(rating)}
                    >
                        <span className="star">&#9733;</span>
                    </button>
                );
            })}
        </div>
    );
};

export class FetchFeedback extends Component {
    //static displayName = FetchFeedback.name;

    constructor(props) {
        super(props);
        this.state = { feedback: [], loading: true };
    }


    componentDidMount() {
        this.populateVideoGameData();
    }


    static renderFeedbackCards(feedback) {
        return (
            <div className="feedback-container">
                {feedback.map(feedbackItem => (
                    <div className="feedback-card" key={feedbackItem.id}>
                        <div className="feedback-text">{feedbackItem.text}</div>
                        <div className="feedback-info">
                            <div>Date: {feedbackItem.date.split("T")[0]}</div>
                            {/*<div>Rating: {feedbackItem.rating}</div>*/}
                            {/*<div>Rating Count: {feedbackItem.rating_count}</div>*/}
                            {/*<div>Flagged: {feedbackItem.is_flagged ? "Yes" : "No"}</div>*/}
                            <div>User: {feedbackItem.account_name}</div>
                            <StarRating />
                        </div>
                    </div>
                ))}
            </div>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : FetchFeedback.renderFeedbackCards(this.state.feedback);

        return (
            <div>
                {/*<h1 id="tabelLabel" >Feedback</h1>*/}
                {/*<p>Product's feedback: </p>*/}
                <br></br>
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