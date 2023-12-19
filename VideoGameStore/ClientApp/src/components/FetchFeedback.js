import React, { Component, useState } from 'react';
import "./FetchFeedback.css";


const StarRating = ({ currentRating, ratingCount, onRate }) => {
    const [hover, setHover] = useState(0);

    const roundedRating = Math.round(currentRating);

    return (
        <div className="star-rating">
            {[...Array(5)].map((star, index) => {
                index += 1;
                return (
                    <button
                        type="button"
                        key={index}
                        className={index <= (hover || roundedRating) ? "on" : "off"}
                        onClick={() => onRate(index)}
                        onMouseEnter={() => setHover(index)}
                        onMouseLeave={() => setHover(roundedRating)}
                    >
                        <span className="star">&#9733;</span>
                    </button>
                );
            })}
            <span>({ratingCount})</span>
        </div>
    );
};


export class FetchFeedback extends Component {
    //static displayName = FetchFeedback.name;

    constructor(props) {
        super(props);
        this.state = {
            feedback: [],
            loading: true,
            newFeedbackText: "",
            username: null,
            userType: null,
            replyText: "",
            replyToId: null,
            showFeedbackInput: false,
        };

        // Bind functions
        this.handleInputChange = this.handleInputChange.bind(this);
        this.submitFeedback = this.submitFeedback.bind(this);
        this.handleReplyChange = this.handleReplyChange.bind(this);
        this.submitReply = this.submitReply.bind(this);
        this.toggleFeedbackInput = this.toggleFeedbackInput.bind(this); // New function binding

    }

    toggleFeedbackInput() {
        this.setState(prevState => ({
            showFeedbackInput: !prevState.showFeedbackInput
        }));
    }

    handleReportClick(feedbackId) {
        if (window.confirm("Are you sure you want to report this feedback?")) {
            this.reportFeedback(feedbackId);
        }
    }

    async reportFeedback(feedbackId) {
        try {
            const response = await fetch(`/api/feedback/report/${feedbackId}`, { method: 'POST' });
            if (response.ok) {
                // Refresh feedback to show updated report count
                this.populateVideoGameData(this.props.productId);
            } else {
                // Handle error
                console.error("Failed to report feedback");
            }
        } catch (error) {
            console.error("Error reporting feedback:", error);
        }
    }


    handleInputChange(event) {
        this.setState({ newFeedbackText: event.target.value });
    }

    handleReplyChange(event) {
        this.setState({ replyText: event.target.value });
    }

    async submitReply(replyingToId, replyText) {
        const searchParams = new URLSearchParams(window.location.search);
        const productId = searchParams.get('id') || null;
        const authCookie = document.cookie.split('; ').find(row => row.startsWith('AuthCookie'));
        const username = authCookie ? authCookie.split('=')[1] : null;

        if (productId && replyText && username && replyingToId != null) {
            const feedback = {
                text: replyText,
                date: new Date().toISOString(),
                rating: 0, // example default value
                rating_count: 0, // example default value
                is_flagged: 0, // example default value
                replying_to_id: replyingToId // replying to this feedback id
            };

            try {
                const response = await fetch(`api/feedback/${productId}`, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ feedback, username }) // Ensure proper JSON structure
                });

                if (response.ok) {
                    this.setState({ replyText: "", replyToId: null });
                    this.populateVideoGameData(productId); // Refresh feedback list
                } else {
                    const errorData = await response.json();
                    console.error('Error submitting reply:', errorData);
                }
            } catch (error) {
                console.error('Error submitting reply:', error);
            }
        } else {
            console.error('Missing information for submitting reply');
        }
    }


    componentDidMount() {
        const searchParams = new URLSearchParams(window.location.search);
        const productId = searchParams.get('id') || null;
        if (productId != null)
            this.populateVideoGameData(productId);

        const authCookie = document.cookie.split('; ').find(row => row.startsWith('AuthCookie'));
        if (authCookie) {
            const username = authCookie.split('=')[1];
            this.setState({ username });

            // Fetch user details to get userType
            fetch(`/api/user/GetUserDetails/${username}`)
                .then(response => response.json())
                .then(data => {
                    this.setState({
                        userType: data.fk_user_type // Set userType based on API response
                    });
                })
                .catch(error => {
                    console.error('Error fetching user details:', error);
                });
        }
    }

    async fetchUserType() {
        const authCookie = document.cookie.split('; ').find(row => row.startsWith('AuthCookie'));
        const username = authCookie ? authCookie.split('=')[1] : null;
        if (username) {
            try {
                const response = await fetch(`/api/user/GetType/${username}`);
                if (response.ok) {
                    const data = await response.json();
                    this.setState({ userType: data.userType });
                }
            } catch (error) {
                console.error('Error fetching user type:', error);
            }
        }
    }

    async submitFeedback() {
        const { newFeedbackText } = this.state;
        const searchParams = new URLSearchParams(window.location.search);
        const productId = searchParams.get('id') || null;
        const authCookie = document.cookie.split('; ').find(row => row.startsWith('AuthCookie'));
        const username = authCookie ? authCookie.split('=')[1] : null;

        if (productId && newFeedbackText && username) {
            const feedback = {
                text: newFeedbackText,
                // Add other required fields, set default values if needed
                date: new Date().toISOString(),
                rating: 0, // example default value
                rating_count: 0, // example default value
                is_flagged: 0, // example default value
            };

            // Call the API to submit the feedback
            try {
                const response = await fetch(`api/feedback/${productId}`, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ feedback, username }) // Ensure proper JSON structure
                });

                if (response.ok) {
                    this.setState({ newFeedbackText: "" });
                    this.populateVideoGameData(productId);
                } else {
                    // Handle non-OK responses
                    const errorData = await response.json();
                    console.error('Error submitting feedback:', errorData);
                }
            } catch (error) {
                console.error('Error submitting feedback:', error);
            }
        }
    }

 /*   getFacebookShareLink(feedbackText, productName, url) {
        const baseUrl = 'https://www.facebook.com/sharer/sharer.php';
        const shareText = `I found this awesome feedback for ${productName} on this online video game store:\n${feedbackText}`;
        return `${baseUrl}?u=${encodeURIComponent(url)}&quote=${encodeURIComponent(shareText)}`;
    }*/

    getFacebookShareLink(url) {
        const baseUrl = 'https://www.facebook.com/sharer/sharer.php';
        return `${baseUrl}?u=${encodeURIComponent(url)}`;
    }

    formatFeedbackForClipboard(productName, feedbackText) {
        return `Found this awesome feedback for "${productName}" on this online video game store:\n\n"${feedbackText}"`;
    }

    copyFeedbackToClipboard(feedbackText) {
        const formattedText = this.formatFeedbackForClipboard(this.props.productName, feedbackText);
        navigator.clipboard.writeText(formattedText).then(() => {
            alert("Feedback copied to clipboard. Please paste it into Facebook.");
        });
    }


    // Add the Report Button and its logic
    renderFeedbackCards(feedback) {
        const { userType, replyToId, replyText, username } = this.state;

        const renderFeedbackOrReply = (item, isReply = false) => (
            <div className={`feedback-card ${isReply ? "reply-card" : ""}`} key={item.id}>
                <div className="feedback-text">{item.text}</div>

                <div className="feedback-info">
                    <div>Date: {item.date.split("T")[0]}</div>
                    <div>User: {item.account_name}</div>
                    {!isReply && username && (
                        <StarRating
                            currentRating={item.rating}
                            ratingCount={item.rating_count}
                            onRate={(newRating) => this.handleRatingUpdate(item.id, newRating)}
                        />
                    )}
                </div>
                {username && !isReply && (
                    <div>
                        <button onClick={() => this.handleReportClick(item.id)}>Report ({item.is_flagged})</button>

                        {(userType === 2 || userType === 3) && (
                            <button onClick={() => this.setState({ replyToId: item.id })}>Reply</button>
                        )}
                        {replyToId === item.id && (
                            <div>
                                <textarea value={replyText} onChange={this.handleReplyChange} />
                                <button onClick={() => this.submitReply(item.id, replyText)}>Submit</button>
                            </div>
                        )}
                        {/*<button onClick={() => window.open(this.getFacebookShareLink(item.text, "Product Name", "https://google.com"), '_blank')}>
                            Share on Facebook
                        </button>*/}
                        <button onClick={() => window.open(this.getFacebookShareLink("https://gameroom.lt/en/"), '_blank')}>
                            Share on Facebook
                        </button>
                        <button onClick={() => this.copyFeedbackToClipboard(item.text)}>Copy Feedback</button>


                    </div>
                )}
            </div>
        );

        return feedback.map(feedbackItem => (
            <div key={feedbackItem.id}>
                {renderFeedbackOrReply(feedbackItem)}
                {feedbackItem.replies && feedbackItem.replies.map(reply => renderFeedbackOrReply(reply, true))}
            </div>
        ));
    }

    async reportFeedback(feedbackId) {
        try {
            const response = await fetch(`/api/feedback/report/${feedbackId}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ feedbackId }) // Send feedbackId in request body
            });

            if (response.ok) {
                alert("Feedback reported successfully.");

                // Update the feedback state to reflect the new report count
                this.setState(prevState => {
                    const updatedFeedback = prevState.feedback.map(item => {
                        if (item.id === feedbackId) {
                            // Assuming 'is_flagged' holds the report count
                            // Increment report count here
                            return { ...item, is_flagged: item.is_flagged + 1 };
                        }
                        return item;
                    });
                    return { feedback: updatedFeedback };
                });

            } else {
                console.error("Failed to report feedback");
                alert("Failed to report feedback.");
            }
        } catch (error) {
            console.error("Error reporting feedback:", error);
            alert("Error reporting feedback.");
        }
    }



    handleReportClick(feedbackId) {
        if (window.confirm("Are you sure you want to report this feedback?")) {
            this.reportFeedback(feedbackId);
        }
    }


    handleRatingUpdate = async (feedbackId, selectedRating) => {
        const newFeedback = this.state.feedback.map(f => {
            if (f.id === feedbackId) {
                const newRatingCount = f.rating_count + 1;
                const newRatingAverage = (f.rating * f.rating_count + selectedRating) / newRatingCount;

                // Optimistically update the rating and rating count
                return { ...f, rating: newRatingAverage, rating_count: newRatingCount };
            }
            return f;
        });

        this.setState({ feedback: newFeedback });

        try {
            const response = await fetch(`/api/feedback/rate/${feedbackId}`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(selectedRating)
            });

            if (!response.ok) {
                // If the API call fails, revert to the original state
                this.populateVideoGameData(this.props.productId);
            }
        } catch (error) {
            console.error("Error updating rating:", error);
            // If there's an error, revert to the original state
            this.populateVideoGameData(this.props.productId);
        }
    };





    render() {
        const { feedback, loading, newFeedbackText, username, userType, showFeedbackInput } = this.state;

        let contents = loading
            ? <p><em>Loading...</em></p>
            : this.renderFeedbackCards(feedback);

        let feedbackInputSection = showFeedbackInput && (
            <div className="feedback-input">
                <textarea
                    className="feedbackTextArea"
                    value={newFeedbackText}
                    onChange={this.handleInputChange}
                    placeholder="Write your feedback here..."
                    maxLength={500}
                    minLength={3}
                />
                <br />
                <button onClick={this.submitFeedback}>Submit Feedback</button>
            </div>
        );

        return (
            <div>
                <div>
                    <br />
                    <h1>User feedback:</h1>
                    <div className="feedback-container">
                        {contents}
                        {username && (userType === 1 || userType === 3) && (
                            <button onClick={this.toggleFeedbackInput}>
                                {showFeedbackInput ? 'Cancel' : 'Create Feedback'}
                            </button>
                        )}
                        {!username && (
                            <p>Please sign up to leave a feedback.</p>
                        ) }
                        {feedbackInputSection}
                    </div>
                    </div>
                <br></br>
                <br></br>
                <br></br>
            </div>

        );
    }

    organizeFeedback(feedbackList) {
        const feedbackMap = {};

        // First, map all feedback by their ID
        feedbackList.forEach(feedback => {
            feedbackMap[feedback.id] = { ...feedback, replies: [] };
        });

        // Then, associate replies with their corresponding feedback
        feedbackList.forEach(feedback => {
            if (feedback.replying_to_id != null) {
                feedbackMap[feedback.replying_to_id].replies.push(feedback);
            }
        });

        // Finally, filter out the replies from the top level, as they are now nested
        return Object.values(feedbackMap).filter(feedback => feedback.replying_to_id == null);
    }

    async populateVideoGameData(productId) {
        //const productId = this.props.productId;
        //const productId = 1;
        const response = await fetch(`api/feedback/${productId}`);
        const data = await response.json();
        const organizedFeedback = this.organizeFeedback(data);
        this.setState({ feedback: organizedFeedback, loading: false });
    }
}