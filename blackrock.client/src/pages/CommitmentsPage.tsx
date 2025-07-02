import * as React from 'react';
import { View, Text, StyleSheet, TouchableOpacity } from 'react-native';

export interface CommitmentData
{
	id: number;
	assetClass: string;
	currency: string;
	amount: number;
}

function CommitmentsPage({ investorId })
{
	const [commitmentData, setCommitmentData] = React.useState<CommitmentData[]>();
	const [investorSummaries, setSummaries] = React.useState <Record<string, number>>();
	const [activeFilter, setActiveFilter] = React.useState<string>('All');
	const [filterButtons, setFilterButtons] = React.useState<string[]>([]);

	async function getCommitmentsData(investorName: string)
	{
		const response = await fetch(`/investorsData/getByInvestor/${investorName}/${activeFilter}`);
		if (response.ok)
		{
			const data = await response.json();
			setCommitmentData(data);
		}
	}

	async function getSummaries(investorName: string)
	{
		const response = await fetch(`/investorsData/getInvestorSummaries/${investorName}`);
		if (response.ok)
		{
			const data = await response.json();
			setSummaries(data);
			if (investorSummaries)
				setFilterButtons(Object.keys(investorSummaries));
		}
	}

	React.useEffect(() =>
	{
		getCommitmentsData(investorId);
	}, [activeFilter]);

	React.useEffect(() =>
	{
		getSummaries(investorId);
	}, [investorSummaries]);


	return (
		<View>
			<View style={styles.container}>
				<View style={styles.buttonContainer}>
					{filterButtons.map((filter) => (
						<TouchableOpacity
							key={filter}							
							style={[styles.filterButton, activeFilter === filter ? styles.activeButton : null]}
							onPress={() => setActiveFilter(filter)}
						>
							<Text style={[styles.buttonText, activeFilter === filter ? styles.activeButtonText : null]}>
								{`${filter} ${amountFormatter.format(investorSummaries[filter])}`}
							</Text>
						</TouchableOpacity>
					))}
				</View>
				<table style={styles.table}>
					<thead style={styles.tableHead}>
						<tr style={styles.tableRow}>
							<th style={styles.tableHeader}>Id</th>
							<th style={styles.tableHeader}>Asset Class</th>
							<th style={styles.tableHeader}>Currency</th>
							<th style={styles.tableHeader}>Amount</th>
						</tr>
					</thead>
					<tbody style={styles.tableBody}>
						{commitmentData?.map((investorData) => (
							<tr key={investorData.id} style={styles.tableRow}>
								<td style={styles.tableCell}>{investorData.id}</td>
								<td style={styles.tableCell}>{investorData.assetClass}</td>
								<td style={styles.tableCell}>{investorData.currency}</td>
								<td style={styles.tableCell}>{amountFormatter.format(investorData.amount)}</td>
							</tr>
						))}
					</tbody>
				</table>
			</View>
		</View>
	);
}

const amountFormatter = Intl.NumberFormat('en', { notation: 'compact' });

const styles = StyleSheet.create({
	container: {
		padding: 20,
	},
	buttonContainer: {
		flexDirection: 'row',
		marginBottom: 20,
		gap: 10,
	},
	filterButton: {
		paddingVertical: 8,
		paddingHorizontal: 16,
		borderRadius: 4,
		backgroundColor: '#f0f0f0',
		borderWidth: 1,
		borderColor: '#ccc',
	},
	activeButton: {
		backgroundColor: '#674d7b',
		borderColor: '#674d7b',
	},
	buttonText: {
		color: '#333',
		fontSize: 14,
		fontWeight: '500',
	},
	activeButtonText: {
		color: '#fff',
	},
	table: {
		width: '100%',
	},
	tableHead: {
		position: 'sticky',
		top: -20,
		backgroundColor: '#a1a1a1',
	},
	tableRow: {
		flexDirection: 'row',
	},
	tableHeader: {
		flex: 1,
		color: 'black',
		fontWeight: 'bold',
		padding: 8,
	},
	tableBody: {
		color: 'black',
	},
	tableCell: {
		flex: 1,
		color: 'black',
		padding: 8,
	},
});

export default CommitmentsPage;