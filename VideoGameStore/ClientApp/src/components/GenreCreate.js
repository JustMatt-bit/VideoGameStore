import React, { Component } from 'react';
import { Routes, Route, useNavigate, Navigate, Link, createSearchParams } from 'react-router-dom';


export class GenreCreate extends Component {
    static displayName = GenreCreate.name;

    constructor(props) {
        super(props);
        this.state = { genreName: '', genreDescription: '', error: [] };

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleChange(event) {
        const target = event.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const name = target.name;
        this.setState({
            [name]: value
        });
    }

    sanitizeString(stringValue) {
        stringValue = stringValue.trim();
        //stringValue = encodeURI(stringValue);
        return stringValue;
    }

    async handleSubmit(event) {
        event.preventDefault();
        this.setState({ error: [] });
        let { genreName, genreDescription } = this.state;
        genreName = this.sanitizeString(genreName);
        genreDescription = this.sanitizeString(genreDescription);
        
        let errors = [];
        //Perform validation
        if (!genreName) {
            errors.push("*Fill in the name of the genre");
        } else if (genreName.length > 30) {
            errors.push("*The name of the genre cannot be over 30 symbols");
        } else {
            const response = await fetch('api/products/GenreExists', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(genreName),
            });
            const data = await response.json();
            if (data) {
                errors.push("*Genre already exists");
            } else {
                errors.push("");
            }
        }
        if (!genreDescription) {
            errors.push("*Fill in the description of the genre");
        } else if (genreDescription.length > 1000) {
            errors.push("*The description cannot be over 1000 symbols");
        }
        if (errors.filter(n => n != '').length == 0) {
            let name = genreName;
            let description = genreDescription;
            const response = await fetch('api/products/CreateGenre', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ name, description }),
            });
            const data = await response.json();
            if (response.ok) {
                window.alert("Genre created successfully");
                window.location.href = '/product-control';
            }
        } else {
            this.setState({ error : errors })
        }
    }

    render() {
        let contents = <></>;
        const { error } = this.state;
        const authCookie = document.cookie
            .split('; ')
            .find(row => row.startsWith('AuthCookie'));

        if (!authCookie) {
            contents = <Navigate to="/fetch-products" replace={true} />
        }
        return (
            <div>
                {contents }
                <form onSubmit={this.handleSubmit}>
                    <div>
                        <div>
                            <label>
                                Genre:
                                <input type="text" name="genreName" value={this.state.genreName} onChange={this.handleChange} />
                            </label>
                            {error[0] && <div style={{ color: 'red' }}>{error[0]}</div>}
                        </div>
                        <br />
                        <div>
                            <label>
                                Description:
                                <textarea name="genreDescription" value={this.state.genreDescription} onChange={this.handleChange} />
                            </label>
                            {error[1] && <div style={{ color: 'red' }}>{error[1]}</div>}
                        </div>
                        <br />
                        <input type="submit" value="Create" />

                    </div>
                </form>

            </div>
        );
    }
}
