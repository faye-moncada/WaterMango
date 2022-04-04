import React, { Component } from 'react';
import { format } from "date-fns";


export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
        this.state = { plants: [], loading: true, timers: [] };
        this.startWatering = this.startWatering.bind(this);
        this.startLog = this.startLog.bind(this);
    }

    async startWatering(plant) {
        if (plant.status === "icon-gray") {
            //do nothing
        }
        else if (plant.status === "icon-darkblue") {
            //cancel timers from initial watering
            clearTimeout(this.state.timers.find(i => i.id === plant.id).gray);
            clearTimeout(this.state.timers.find(i => i.id === plant.id).blue);

            //compute for remaining seconds before plant can be watered again
            var prev = new Date(plant.lastWatered).getSeconds() + 30;
            var now = new Date().getSeconds();
            var remainingTime = (prev - now) * 1000;
            //console.log(prev + "  " + now + "  " + remainingTime);

            this.setState(prevState => ({ plants: prevState.plants.map(el => (el.id === plant.id ? { ...el, status: "icon-gray" } : el)) }));
            setTimeout(() => {
                this.setState(prevState => ({ plants: prevState.plants.map(el => (el.id === plant.id ? { ...el, status: "icon-blue" } : el)) }));
            }, remainingTime)
        }
        else {
            const response = await fetch(`plants/${plant.id}`,
                {
                    method: "put",
                    headers:
                    {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify(plant)
                });
            if (response.ok) {
                const refresh = await fetch('plants');
                const data = await refresh.json();
                this.setState({ plants: data });
                var grayTimer = setTimeout(() => {
                    this.setState(prevState => ({ plants: prevState.plants.map(el => (el.id === plant.id ? { ...el, status: "icon-gray" } : el)) }));
                }, 10000);
                var blueTimer = setTimeout(() => {
                    this.setState(prevState => ({ plants: prevState.plants.map(el => (el.id === plant.id ? { ...el, status: "icon-blue" } : el)) }));
                }, 20000);
                var t = { id: plant.id, gray: grayTimer, blue: blueTimer };

                //record timer id in case it has to be cancelled 
                this.setState(prevState => ({ timers: prevState.timers.map(el => (el.id === plant.id ? { ...el, gray: grayTimer, blue: blueTimer } : el )) }));
            }
        }
    }

    startLog(p) {
        console.log(p.name);
    }

    componentDidMount() {
        this.populatePlantData();
    }

    static renderPlantsTable(plants,context) {
        return (
            <table className='table' aria-labelledby="tableLabel">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Description</th>
                        <th>Last Watered</th>
                        <th>Image</th>
                        <th>Status</th>
                    </tr>
                </thead>
                <tbody>
                    {plants.map(plant =>
                        <tr key={plant.id}>
                            <td>{plant.name}</td>
                            <td>{plant.description}</td>
                            <td>{format(new Date(plant.lastWatered),"MMMM dd, yyyy h:mm:ss a")}</td>
                            <td><img alt="" className={plant.status === "icon-red" ? "image-grayscale" : "image-color" } src={`data:image/jpeg;base64,${plant.image}`} width="200" height="200"/></td>
                            <td><button className="button" onClick={() => context.startWatering(plant)}><span className={plant.status}></span></button></td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Home.renderPlantsTable(this.state.plants,this);

        return (
            <div>
                <h3 id="tableLabel" >Office Plants</h3>
                {contents}
            </div>
        );
    }

    async populatePlantData() {
        const response = await fetch('plants');
        const data = await response.json();
        var ts = data.map(plant => { return { id: plant.id, gray: null, blue: null } });
        this.setState({ plants: data, loading: false, timers: ts });
       
    }

}
