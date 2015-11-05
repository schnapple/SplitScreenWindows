import java.awt.BorderLayout;
import java.awt.Dimension;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

import javax.swing.JButton;
import javax.swing.JFrame;
//import javax.swing.JLabel;
//import javax.swing.SwingUtilities;

public class SplitScreenGUI{

	public static void main(String[] args) {
		// TODO Auto-generated method stub
		JFrame frame = new JFrame("SplitScreen");
		frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		//JLabel emptyLabel = new JLabel("");
		frame.setPreferredSize(new Dimension(500, 300));
		JButton button = new JButton("Run C++");
		button.addActionListener(new ActionListener(){
			public void actionPerformed(ActionEvent e){
				MouseMotionTracker mouseMotion = new MouseMotionTracker();
			}
		});
		//frame.pack();
		frame.getContentPane().add(button, BorderLayout.CENTER);
		frame.update(null);
		frame.pack();
		frame.setVisible(true);
	}

}
