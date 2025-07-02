import React from 'react';
import { View, Text, StyleSheet, TouchableOpacity, Modal } from 'react-native';
import CommitmentsPage from './CommitmentsPage';

export interface InvestorData
{
	id: number;
	name: string;
	type: string;
	dateAdded: string;
	address: string;
	totalCommitment: number;
}

const InvestorsPage: React.FC = () =>
{
	const [investorsData, setInvestorData] = React.useState<InvestorData[]>();
	const [selectedInvestor, setSelectedInvestor] = React.useState<string | null>(null);
	const [modalVisible, setModalVisible] = React.useState(false);

	async function getInvestorData()
	{
		const response = await fetch('/investorsData/getSampleData');
		if (response.ok)
		{
			const data = await response.json();
			setInvestorData(data);
		}
	}

	React.useEffect(() =>
	{
		getInvestorData();
	}, []);

	const handleRowClick = (investorName: string) =>
	{
		setSelectedInvestor(investorName);
		setModalVisible(true);
	};

	const closeModal = () =>
	{
		setModalVisible(false);
		setSelectedInvestor(null);
	};

	return (
		<View>
			<View style={styles.container}>
				<h1 style={styles.screenText}>Investors</h1>
				<table style={{ width: '100%' }}>
					<thead style={styles.stickyHeader}>
						<tr>
							<th>Id</th>
							<th>Name</th>
							<th>Type</th>
							<th>Date added</th>
							<th>Address</th>
							<th>Total Commitment</th>
						</tr>
					</thead>
					<tbody style={{ color: 'black' }}>
						{investorsData?.map((investorData) => (
							<tr
								className="investors"
								key={investorData.id}
								onClick={() => handleRowClick(investorData.name)}
								style={styles.tableRow}
							>
								<td>{investorData.id}</td>
								<td>{investorData.name}</td>
								<td>{investorData.type}</td>
								<td>{investorData.dateAdded}</td>
								<td>{investorData.address}</td>
								<td>{amountFormatter.format(investorData.totalCommitment)}</td>
							</tr>
						))}
					</tbody>
				</table>
			</View>

			<Modal
				animationType="slide"
				transparent={true}
				visible={modalVisible}
				onRequestClose={closeModal}
			>
				<View style={styles.modalOverlay}>
					<View style={styles.modalContent}>
						<View style={styles.headerContainer}>
							<h1 style={styles.screenText}>
								Breakdown of commitments for {selectedInvestor || 'Investor'}
							</h1>
							<TouchableOpacity style={styles.closeButton} onPress={closeModal}>
								<Text style={styles.closeButtonText}>Close</Text>
							</TouchableOpacity>
						</View>
						<CommitmentsPage investorId={selectedInvestor}></CommitmentsPage>
					</View>
				</View>
			</Modal>
		</View>
	);
};

const amountFormatter = Intl.NumberFormat('en', { notation: 'compact' });

const styles = StyleSheet.create({
	stickyHeader:
	{
		color: 'black',
		position: 'sticky',
		top: -20,
		backgroundColor: '#a1a1a1'
	},
	container: {
		height: 'calc(100vh - 120px)',
		overflow: 'overlay',
		padding: 20,
	},
	screenText: {
		fontSize: 30,
		fontWeight: 'bold',
		color: '#333',
		marginBottom: 20,
		marginTop: 0,
	},
	tableRow: {
		cursor: 'pointer',
		transition: 'all 0.1s ease',
	},
	modalOverlay: {
		flex: 1,
		backgroundColor: 'rgba(0, 0, 0, 0.9)',
		justifyContent: 'center',
		alignItems: 'center',
	},
	modalContent: {
		backgroundColor: 'white',
		borderRadius: 10,
		padding: 20,
		width: '100%',
		maxWidth: 1200,
		maxHeight: '90vh',
		overflow: 'overlay',
		position: 'fixed',
		top: 80,
	},
	headerContainer: {
		flexDirection: 'row',
		justifyContent: 'space-between',
		alignItems: 'center',
		marginBottom: 20,
	},
	closeButton: {
		backgroundColor: '#b58888',
		padding: 5,
		borderRadius: 10,
		width: 80,
		alignItems: 'center',
	},
	closeButtonText: {
		fontWeight: '600',
		color: 'black',
	},
});

export default InvestorsPage;