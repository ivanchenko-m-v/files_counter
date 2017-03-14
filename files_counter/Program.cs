using System;
using System.IO;
using System.Text;

namespace files_counter
{
    class Program
    {
        static void Main( string[] args )
        {
            if( args.Length < 1 )
            {
                Console.WriteLine( "wrong program arguments count" );
                Console.WriteLine( "usage: file_counter.exe <FILE_MASK> [PATH]" );
                Console.WriteLine( "\t<FILE_MASK>\t - required param: file type template (as example '*.pdf')" );
                Console.WriteLine( "\t[PATH]\t - optional param: path for search files" );
                return;
            }
            string s_filter = args[0];
            string s_path = args.Length > 1 ? args[1] : "";
            try
            {
                //
                Program.process_directory( s_path, s_filter );
                //

            }
            catch( Exception ex )
            {
                Console.WriteLine( "{0}: {1}", ex.GetType( ), ex.Message );
            }
            Console.WriteLine( "finished!" );
            Console.WriteLine( "press any key for exit..." );
            Console.ReadKey( );
        }

        static void process_directory(
                                        string s_path = "",
                                        string s_filter = "*.*"
                                     )
        {
            string s_dir_current = Directory.GetCurrentDirectory( );
            string start_path = s_path.Length > 0 ? s_path : s_dir_current;
            if( !Directory.Exists( start_path ) )
            {
                Console.WriteLine( "The directory '{0}' doesn't exist", start_path );
                return;
            }
            //open file
            using( StreamWriter bw = File.CreateText( s_dir_current + "\\output.csv" ) )
            {
                //write header
                bw.WriteLine( "{0};{1};{2}", "Path:", "Files Count:", "Files Size:" );
                //count files and write files info
                Program.write_dirinfo_to_csv( new DirectoryInfo( start_path ), bw, s_filter );
            }
        }

        static void write_dirinfo_to_csv( DirectoryInfo di, StreamWriter bw, string s_filter )
        {
            Console.WriteLine( "{0}", di.FullName );

            DirectoryInfo[] di_children = di.GetDirectories( );
            foreach( DirectoryInfo di_child in di_children )
            {
                write_dirinfo_to_csv( di_child, bw, s_filter );
            }
            //get files info
            FileInfo[] fi_children = di.GetFiles( s_filter );
            if( fi_children.Length == 0 )
            {
                return;
            }
            //count files size
            long f_sz = 0;
            foreach( FileInfo fi in fi_children)
            {
                f_sz += fi.Length;
            }
            bw.WriteLine( "{0};{1};{2}", di.FullName, fi_children.Length, f_sz );
        }

    }//class Program

}//namespace files_counter
