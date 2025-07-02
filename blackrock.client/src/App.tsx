import * as React from 'react';
import './App.css';
import { View, Text, StyleSheet, TouchableOpacity } from 'react-native';
import brLogo from './assets/br_logo.svg';
import InvestorsPage from './pages/InvestorsPage';
import CommitmentsPage from './pages/CommitmentsPage';
import AboutPage from './pages/aboutPage';


function App()
{
	const [activeTab, setActiveTab] = React.useState<'Investors' | 'About'>('Investors');
	
	return (
		<View style={styles.container}>
			<View style={styles.topBarContainer}>
				<img style={styles.logoImage} src={brLogo} />
				<View style={styles.tabBar}>
					<View style={styles.tabButtonsContainer}>
						<TouchableOpacity
							style={[styles.tabButton, activeTab === 'Investors' ? styles.activeTab : null]}
							onPress={() => setActiveTab('Investors')}
						>
							<Text style={[styles.tabText, activeTab === 'Investors' ? styles.activeTabText : null]}>
								Investors
							</Text>
						</TouchableOpacity>
						<TouchableOpacity
							style={[styles.tabButton, activeTab === 'About' ? styles.activeTab : null]}
							onPress={() => setActiveTab('About')}
						>
							<Text style={[styles.tabText, activeTab === 'About' ? styles.activeTabText : null]}>
								About
							</Text>
						</TouchableOpacity>
					</View>
				</View>
			</View>
			<View>
				{activeTab === 'Investors' ? <InvestorsPage /> : <AboutPage />}
			</View>
		</View >
	);
}

const styles = StyleSheet.create({
	logoImage: {
		width: 100,
		paddingLeft: 10,
		filter: 'invert(1)'
	},
	container: {
		flex: 1,
		backgroundColor: '#F5F5F5',
		height: '100%'
	},
	topBarContainer: {
		display: 'flex',
		flexDirection: 'row',
		backgroundColor: '#1c1c1c',
		height:60
	},
	tabBar: {
		flexDirection: 'row',
		paddingTop: 10,
		paddingBottom: 10,
		justifyContent: 'center',
		flex: 1
	},
	tabButtonsContainer:
	{
		display: 'flex',
		flexDirection: 'row',
		width: 200
	},
	tabButton: {
		flex: 1,
		paddingVertical: 10,
		alignItems: 'center',
		justifyContent: 'center',
		marginRight: 10
	},
	activeTab: {
		borderBottomWidth: 3,
		borderBottomColor: '#c181f1',
	},
	tabText: {
		fontSize: 14,
		fontWeight: 'bold',
		color: '#F8F8F8',
	},
	activeTabText: {
		color: '#c181f1',
	},
	screenContainer: {
		flex: 1,
		justifyContent: 'center',
		alignItems: 'center',
		backgroundColor: '#F5F5F5',
	},
	screenText: {
		fontSize: 30,
		fontWeight: 'bold',
		color: '#333',
		marginBottom: 20,
		marginTop:0
	},
	subText: {
		fontSize: 16,
		color: '#666',
		textAlign: 'center',
		paddingHorizontal: 20,
	},
});

export default App;