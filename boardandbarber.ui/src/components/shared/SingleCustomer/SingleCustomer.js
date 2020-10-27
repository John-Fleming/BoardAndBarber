import React from 'react';
import "./SingleCustomer.scss";

class SingleCustomer extends React.Component {
    render() {
        const {customer} = this.props;
        return (
            <>
            <strong>{customer.name}</strong>
                <ul>
                    <li>Id: {customer.id}</li>
                    <li>Birthday: {customer.birthday}</li>
                    <li>Favorite Barber: {customer.favoriteBarber}</li>
                    <li>Notes: {customer.notes}</li>
                </ul>
            </>
        )
    }
}

export default SingleCustomer;