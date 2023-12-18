import React, { Component } from 'react';
import { Routes, Route, useNavigate, useSearchParams } from 'react-router-dom';
import { FetchFeedback } from './FetchFeedback';

function ToCart() {
    const navigate = useNavigate();
    const navigateToCart = () => {
        navigate('/cart');
    };
    return (
        <div>
            <button onClick={navigateToCart}>Add to cart</button>
        </div>
    );
}
export class Product extends Component {
    static displayName = Product.name;
    constructor(props) {
        super(props);

        
        this.state = {
            loading: true,
            data: null,
            hasID: false
        };
    }

    componentDidMount() {
        const searchParams = new URLSearchParams(window.location.search);
        if (searchParams.has('id')) {

            const id = searchParams.get('id');
            this.getData(id);
        } else {
            this.setState({ loading: false, data: null, hasID: false });
        }
    }

    static productRender(product) {
        return (
            <>
                <img style={{ float: 'left', margin: '0 40px 0 40px', width: '30%', height: '30%' }} src={`/images/${product.image}`} />
                <div style={{ margin: '0 40px 0 40px' }}>
                    
                    <h2>{product.name}</h2>
                    <p>{product.description}
                    </p>
                    <h4>Price: {product.price} €</h4>
                    {/*<ToCart/>*/}
                    <button>Add to cart</button>

                </div>
                <div style={{ clear: 'both' }}></div>
                <FetchFeedback />
            </>
        );
    }

    render() {
        const { data, loading} = this.state;
        
        if (data == null && !loading) {
            window.location.href = '/fetch-products';
            return null;
        }
        let contents = loading
            ? <p><em>Loading...</em></p>
            : Product.productRender(data);
        return (
            <div>
                {contents}
            </div>

        );
    }

    async getData(id) {
        try {
            const response = await fetch(`api/products/GetProduct/${id}`);
            const prodData = await response.json();
            this.setState({ loading: false, data: prodData, hasID: true });
        }
        catch (error) {
            this.setState({ loading: false, data: null, hasID: false });
        }
    }
}