import * as React from 'react';
import './App.css';
import { NavigationContainer } from '@react-navigation/native';
import { createMaterialTopTabNavigator } from '@react-navigation/material-top-tabs';
import { View, Text, StyleSheet } from 'react-native';
const Stack = createMaterialTopTabNavigator();
// Create Tab Navigator
const Tab = createMaterialTopTabNavigator();
// Investors Component
const InvestorsScreen = () => {
    return (<View style={styles.screenContainer}>
            <Text style={styles.screenText}>Investors Page</Text>
            <Text style={styles.subText}>Manage investor information and details here.</Text>
        </View>);
};
// Commitments Component
const CommitmentsScreen = () => {
    return (<View style={styles.screenContainer}>
            <Text style={styles.screenText}>Commitments Page</Text>
            <Text style={styles.subText}>View and track commitments here.</Text>
        </View>);
};
function App() {
    const [forecasts, setForecasts] = React.useState();
    React.useEffect(() => {
        getInvestorData();
    }, []);
    const contents = forecasts === undefined
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
        : <table className="table table-striped" aria-labelledby="tableLabel">
            <thead>
                <tr>
                    <th>Date</th>
                    <th>Temp. (C)</th>
                    <th>Temp. (F)</th>
                    <th>Summary</th>
                </tr>
            </thead>
            <tbody>
                {forecasts.map(forecast => <tr key={forecast.date}>
                        <td>{forecast.date}</td>
                        <td>{forecast.temperatureC}</td>
                        <td>{forecast.temperatureF}</td>
                        <td>{forecast.summary}</td>
                    </tr>)}
            </tbody>
        </table>;
    return (<NavigationContainer>
            <Tab.Navigator screenOptions={{
            tabBarActiveTintColor: '#FFFFFF',
            tabBarInactiveTintColor: '#F8F8F8',
            tabBarStyle: {
                backgroundColor: '#6200EE',
            },
            tabBarLabelStyle: {
                textAlign: 'center',
                fontSize: 14,
                fontWeight: 'bold',
            },
            tabBarIndicatorStyle: {
                backgroundColor: '#FFFFFF',
                height: 3,
            },
        }}>
                <Tab.Screen name="Investors" component={InvestorsScreen} options={{ tabBarLabel: 'Investors' }}/>
                <Tab.Screen name="Commitments" component={CommitmentsScreen} options={{ tabBarLabel: 'Commitments' }}/>
            </Tab.Navigator>
        </NavigationContainer>);
    async function getInvestorData() {
        const response = await fetch('/investorsData/getSampleData');
        if (response.ok) {
            const data = await response.json();
            setForecasts(data);
        }
    }
}
const styles = StyleSheet.create({
    screenContainer: {
        flex: 1,
        justifyContent: 'center',
        alignItems: 'center',
        backgroundColor: '#F5F5F5',
    },
    screenText: {
        fontSize: 24,
        fontWeight: 'bold',
        color: '#333',
        marginBottom: 10,
    },
    subText: {
        fontSize: 16,
        color: '#666',
        textAlign: 'center',
        paddingHorizontal: 20,
    },
});
export default App;
