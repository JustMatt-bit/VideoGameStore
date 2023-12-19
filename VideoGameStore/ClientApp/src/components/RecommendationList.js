import React, { Component } from 'react';
import { Routes, Route, useNavigate, Navigate, Link, createSearchParams } from 'react-router-dom';


function ToCreateNew() {
    const toNewList = () => {
        window.location.reload(false);
    };
    return (
        <div style={ {textAlign: 'center', marginBottom: '20px'}} >
            <button style={{ border: 'solid 2px black' }} onClick={toNewList}>Generate new recommendation</button>
        </div>
    );
}


export class RecommendationList extends Component {
    static displayName = RecommendationList.name;

    constructor(props) {
        super(props);
        this.state = { products: [], loading: true };
    }

    componentDidMount() {
        this.populateData();
    }

    async populateData() {
        const authCookie = document.cookie
            .split('; ')
            .find(row => row.startsWith('AuthCookie'));

        if (authCookie) {
            const authCookieValue = authCookie.split('=')[1];
            const username = authCookieValue;
            const response = await fetch(`/api/recommendation/get/${username}`);
            const data = await response.json();
            this.setState({ products: data, loading: false, name: username });
        }

    }


    static renderRecommendationList(products) {
        return (
            <div>
                    {products.map(product =>
                        <>
                            
                            <div style={{ margin: ' 20px auto', width: '18%' }}>
                                <Link style={{ color: 'black', textDecoration: 'none' }} target="_blank" to={{
                                    pathname: "/product",
                                    search: `?${createSearchParams({
                                        id: product.id
                                    })}`
                                }}>
                                    <div style={{ border: '1px black solid' }}>
                                        <img style={{ height: '300px', width: '100%', margin: ' auto' }} src={`/images/${product.image}`} />
                                        <div style={{ width: '50%', margin: ' auto', textAlign: 'center' }}>
                                            <h6 >{product.name}</h6>
                                            <p>Price: {product.price} €</p>
                                        </div>
                                    </div>
                                </Link>
                                </div>
                            
                        </>
                    )}
            </div>
        );
    }

    render() {
        let contents;
        let startingContent = <h1 id="tabelLabel" >Recommended products</h1>;
        if (this.state.products.length == 0) {
            contents = this.state.loading
                ? <p><em>Loading...</em></p>
                : <p style={{ textAlign: 'center', lineHeight: '100px' }}>No products</p>;
        } else {
            contents = this.state.loading
                ? <p style={{ textAlign: 'center', lineHeight: '100px' }}><em>Loading...</em></p>
                : RecommendationList.renderRecommendationList(this.state.products);
        }
        contents = <>
            {startingContent}
            {contents}
            <ToCreateNew />
        </>;
        return (
            <div>
                {contents}
            </div>
        );
    }
}